using CDWRepository;

namespace CDWSVCAPI.Helpers
{
    public class BLHelper
    {
        public static bool isValidUser(CDWSVCUser user, string hash = "")
        {
            // does the user id (guid) equal the hash un pgp'd with a config specified salt?
            return user != null;
        }
    }
}
