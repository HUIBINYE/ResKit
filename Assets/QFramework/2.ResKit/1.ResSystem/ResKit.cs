using System.IO;
using QFramework.ResKit;

namespace QFramework
{
    public class ResKitConfig
    {
        #region AssetBundle 相关

        public const string ABMANIFEST_ASSET_NAME = "assetbundlemanifest";

        public static string AB_RELATIVE_PATH
        {
            get { return "AssetBundles/" + PathUtil.GetPlatformName() + "/"; }
        }

        public static string AssetBundleUrl2Name(string url)
        {
            string retName = null;
            string parren = FilePath.StreamingAssetsPath + "AssetBundles/" + PathUtil.GetPlatformName() + "/";
            retName = url.Replace(parren, "");

            parren = FilePath.PersistentDataPath + "AssetBundles/" + PathUtil.GetPlatformName() + "/";
            retName = retName.Replace(parren, "");
            return retName;
        }

        public static string AssetBundleName2Url(string name)
        {
            string retUrl = FilePath.PersistentDataPath + "AssetBundles/" + PathUtil.GetPlatformName() + "/" + name;
			
            if (File.Exists(retUrl))
            {
                return retUrl;
            }
			
            return FilePath.StreamingAssetsPath + "AssetBundles/" + PathUtil.GetPlatformName() + "/" + name;
        }

        //导出目录
        public static string EDITOR_AB_EXPORT_ROOT_FOLDER
        {
            get { return "StreamingAssets/AssetBundles/" + RELATIVE_AB_ROOT_FOLDER; }
        }

        /// <summary>
        /// AssetBundle存放路径
        /// </summary>
        public static string RELATIVE_AB_ROOT_FOLDER
        {
            get { return "/AssetBundles/" + PathUtil.GetPlatformName() + "/"; }
        }

        /// <summary>
        /// AssetBundle 配置路径
        /// </summary>
        public static string EXPORT_ASSETBUNDLE_CONFIG_FILENAME
        {
            get { return "asset_bindle_config.bin"; }
        }

        #endregion
    }
}