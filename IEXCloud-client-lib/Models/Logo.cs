using IEXCloudClient.NetCore.Helper;

namespace IEXCloudClient.NetCore
{
    public interface ILogo
    {
        string URL { get; set; }
    }

    public class Logo : ILogo
    {
        public string URL { get; set; }

		public override string ToString(){
			return this.ToTableString();
		}
    }

}
