using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    public class TextureConvertorASTC : AssetPostprocessor
    {
        void OnPostprocessTexture(Texture2D texture)
        {
            TextureImporter importer = assetImporter as TextureImporter;
        
            if(importer==null) return;
        
            TextureImporterPlatformSettings platformSettings= importer.GetPlatformTextureSettings("iPhone");
            platformSettings.overridden = true;
            platformSettings.format = TextureImporterFormat.ASTC_8x8;
            platformSettings.textureCompression = TextureImporterCompression.CompressedHQ;
            platformSettings.compressionQuality = 100;

            importer.SetPlatformTextureSettings(platformSettings);
            // importer.compressionQuality = 2;

        }
    }
}