using System;

public class Signer : ISigner
{
		public SigningData GenerateSigningData(string request_method, string canonical_uri, string canonical_querystring, string access_key, string secret_key, string host){
			var utcNow = DateTime.UtcNow;
            var iexdate = utcNow.ToString("yyyyMMddTHHmmss") + "Z";
			var datestamp = utcNow.ToString("yyyyMMdd");
			//Console.WriteLine(iexdate);
			//Console.WriteLine(datestamp);
			var canonical_headers = "host:" + host + "\n" + "x-iex-date:" + iexdate + "\n";
			var signed_headers = "host;x-iex-date";
			var payload = "";
			var payload_hash = CreateHash(payload);
			var canonical_request = request_method + "\n" + canonical_uri + "\n" + canonical_querystring + "\n" + canonical_headers + "\n" + signed_headers + "\n" + payload_hash;
			//Console.WriteLine(canonical_request);

			var algorithm = "IEX-HMAC-SHA256";
			var credential_scope = datestamp + "/" + "iex_request";
			var string_to_sign = algorithm + "\n" +  iexdate + "\n" +  credential_scope + "\n" + CreateHash(canonical_request);
            var signing_key = CreateSignatureKey(secret_key, datestamp);
			//Console.WriteLine(string_to_sign);
			//Console.WriteLine(signing_key);
			var signature = CreateHmac(signing_key, string_to_sign);

			//Console.WriteLine(signature);
            var authorization_header = algorithm + " " + "Credential=" + access_key + "/" + credential_scope + ", " +  "SignedHeaders=" + signed_headers + ", " + "Signature=" + signature;
			//Console.WriteLine(authorization_header);

			return new SigningData() {
				authorization_header = authorization_header,
				iexdate = iexdate
			};
		}
        
		private static string CreateHash(string input)
		{
			var myEncoder = new System.Text.UTF8Encoding();
			var Text = myEncoder.GetBytes(input);
			var myHMACSHA1 = new System.Security.Cryptography.SHA256Managed();
			var HashCode = myHMACSHA1.ComputeHash(Text);
			var hash =  BitConverter.ToString(HashCode).Replace("-", "");
			return hash.ToLower();
		}
		
		private static string CreateHmac(string secret, string data)
		{
			var myEncoder = new System.Text.UTF8Encoding();
			var secretBytes = myEncoder.GetBytes(secret);
			var dataBytes = myEncoder.GetBytes(data);
			var myHMACSHA1 = new System.Security.Cryptography.HMACSHA256(secretBytes);
			var HashCode = myHMACSHA1.ComputeHash(dataBytes);
			var hash =  BitConverter.ToString(HashCode).Replace("-", "");
			return hash.ToLower();
		}

		private static string Sign(string secret, string data) {
			return CreateHmac(secret, data);
		}

		private static string CreateSignatureKey(string key, string datestamp) {
			var signedDate = Sign(key, datestamp);
			return Sign(signedDate, "iex_request");
		}

}

public interface ISigner
{
    SigningData GenerateSigningData(string request_method, string canonical_uri, string canonical_querystring, string access_key, string secret_key, string host);
}

public struct SigningData {
    public string iexdate;
    public string authorization_header;
}