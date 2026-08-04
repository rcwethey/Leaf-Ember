using System;
using UnityEditor;

namespace LeafEmber.Editor
{
    /// <summary>
    /// Keeps the imported environment library game-ready and deterministic.
    /// Source assets remain at 1K, normal/packed maps stay linear, and foliage
    /// opacity survives compression without making textures CPU-readable.
    /// </summary>
    public sealed class EnvironmentTextureImportPolicy : AssetPostprocessor
    {
        private const string EnvironmentRoot = "Assets/LeafEmber/Resources/Environment/";

        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(EnvironmentRoot, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            TextureImporter importer = (TextureImporter)assetImporter;
            string lowerPath = assetPath.ToLowerInvariant();

            importer.maxTextureSize = 1024;
            importer.isReadable = false;
            importer.mipmapEnabled = true;
            importer.textureCompression = TextureImporterCompression.CompressedHQ;

            if (lowerPath.Contains("_nor_gl_"))
            {
                importer.textureType = TextureImporterType.NormalMap;
                importer.sRGBTexture = false;
                return;
            }

            if (lowerPath.Contains("_arm_") || lowerPath.Contains("_alpha_"))
            {
                importer.sRGBTexture = false;
            }

            if (lowerPath.EndsWith(".png", StringComparison.Ordinal))
            {
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.alphaIsTransparency = lowerPath.Contains("leaves")
                    || lowerPath.Contains("branches")
                    || lowerPath.Contains("_alpha_");
            }
        }
    }
}
