#!/usr/bin/env node

/*
Copyright 2019-2020 iexcloud. or its affiliates. All Rights Reserved.

This file is licensed under the Apache License, Version 2.0 (the "License").
You may not use this file except in compliance with the License. A copy of the
License is located at

https://www.apache.org/licenses/LICENSE-2.0

This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS
OF ANY KIND, either express or implied. See the License for the specific
language governing permissions and limitations under the License.
*/

module.exports = function(callback, method, canonical_uri, canonical_querystring, access_key, secret_key, host) { 
    const moment = require('moment');
    const crypto = require('crypto');
    const https  = require('https');
    
    var ts = moment.utc();
    const iexdate = ts.format("YYYYMMDDTHHmmss") + 'Z';
    const datestamp = ts.format("YYYYMMDD");
    
    function sign(secret, data) {
        return crypto.createHmac('sha256', secret).update(data, "utf8").digest('hex');
    };
    
    function getSignatureKey(key, datestamp) {
        const signedDate = sign(key, datestamp);
        return sign(signedDate, 'iex_request');
    }
    
    if ( ! access_key || ! secret_key ) {
        console.warn('No access key is available.')
        process.exit(1);
    }
    
    const canonical_headers = 'host:' + host + '\n' + 'x-iex-date:' + iexdate + '\n';
    const signed_headers = 'host;x-iex-date'
    const payload = '';
    const payload_hash = crypto.createHash('sha256').update(payload).digest('hex');
    const canonical_request = method + '\n' + canonical_uri + '\n' + canonical_querystring + '\n' + canonical_headers + '\n' + signed_headers + '\n' + payload_hash;
    const algorithm = 'IEX-HMAC-SHA256';
    const credential_scope = datestamp + '/' + 'iex_request';
    const string_to_sign = algorithm + '\n' +  iexdate + '\n' +  credential_scope + '\n' + crypto.createHash('sha256').update(canonical_request, "utf8").digest('hex');
    const signing_key = getSignatureKey(secret_key, datestamp)
    const signature = crypto.createHmac('sha256', signing_key).update(string_to_sign, "utf8").digest('hex');
    const authorization_header = algorithm + ' ' + 'Credential=' + access_key + '/' + credential_scope + ', ' +  'SignedHeaders=' + signed_headers + ', ' + 'Signature=' + signature
    const headers = {'x-iex-date':iexdate, 'Authorization':authorization_header}

    callback(null, headers); 
};