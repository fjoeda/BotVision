using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BotUnyuu.Computer_Vision
{
    public class Vision
    {
        // Variabel untuk key dan objek computer vision
        private const string subscriptionKey = "Masukkan key anda disini";
        private ComputerVisionClient computerVision;

        // Singleton
        private static Vision _instance;
        public static  Vision Instance { get
            {
                if (_instance == null)
                    _instance = new Vision();
                return _instance;
                    
            }
        }
        private Vision() {
            computerVision = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(subscriptionKey),
                    new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://southeastasia.api.cognitive.microsoft.com";
        }


        // Spesifikasi Fitur yang ingin di dapatkan
        private static readonly List<VisualFeatureTypes> features =
            new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags
        };

        // Method untuk mendapatkan caption gambar
        public async Task<string> GetImageCaption(string imageUrl)
        {
            string captionGambar;
            Stream Gambar;
            using(var httpClient = new HttpClient())
            {
                var uri = new Uri(imageUrl);
                Gambar = await httpClient.GetStreamAsync(uri);
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(Gambar,features);
                captionGambar = analysis.Description.Captions[0].Text;
            }          
            
            return captionGambar;

        }

    }
}
