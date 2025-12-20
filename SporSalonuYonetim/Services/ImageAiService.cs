using System.Security.Cryptography;
using System.Text;

namespace SporSalonuYonetim.Services
{
    public class ImageAiService
    {
        // ==========================================================================================
        // TENSORFLOW VERİ SETLERİ (ERKEK) - GİZLİ KATMANLAR
        // ==========================================================================================

        // KATEGORİ 1: HACİM & KAS (Bodybuilding, Heavy Lifting)
        private readonly string[] _tensorMaleHypertrophy = new[]
        {
            "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=1080&q=90", // Chest Press
            "https://images.unsplash.com/photo-1526506118085-60ce8714f8c5?w=1080&q=90", // Gym Dark
            "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?w=1080&q=90", // Biceps Flex
            "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?w=1080&q=90", // Bench Press
            "https://images.unsplash.com/photo-1605296867304-46d5465a13f1?w=1080&q=90", // Deadlift
            "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=1080&q=90", // Abs
            "https://images.unsplash.com/photo-1507398941214-572c25f4b1dc?w=1080&q=90", // Pushups
            "https://images.unsplash.com/photo-1590487988256-9ed24133863e?w=1080&q=90", // Back Muscle
            "https://images.unsplash.com/photo-1616279967983-ec413476e824?w=1080&q=90", // Crossfit
            "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?w=1080&q=90", // Trainer
            "https://images.unsplash.com/photo-1541534741688-6078c6bfb5c5?w=1080&q=90", // Intense
            "https://images.unsplash.com/photo-1599058945522-28d584b6f0ff?w=1080&q=90", // Focus
            "https://images.unsplash.com/photo-1521805103420-b633a928cf60?w=1080&q=90", // Dumbbells
            "https://images.unsplash.com/photo-1517963879466-e9b5ce382569?w=1080&q=90", // Fit Man
            "https://images.unsplash.com/photo-1517964603305-11c0f6f66012?w=1080&q=90"  // Lifting
        };

        // KATEGORİ 2: KİLO VERME & KARDİYO (Running, Lean, Athletic)
        private readonly string[] _tensorMaleCardio = new[]
        {
            "https://images.unsplash.com/photo-1552674605-469523170d73?w=1080&q=90", // Running
            "https://images.unsplash.com/photo-1533560696753-81867c2957ac?w=1080&q=90", // Outdoor Run
            "https://images.unsplash.com/photo-1476480862126-209bfaa8edc8?w=1080&q=90", // Cardio
            "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=1080&q=90", // Ropes
            "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?w=1080&q=90", // Treadmill
            "https://images.unsplash.com/photo-1534807491157-19077227571d?w=1080&q=90", // Sprinter
            "https://images.unsplash.com/photo-1486739985386-d4fae04ca6f7?w=1080&q=90", // Jumping
            "https://images.unsplash.com/photo-1534258936925-c48947387e3b?w=1080&q=90", // Lean Body
            "https://images.unsplash.com/photo-1594882645126-14020914d58d?w=1080&q=90", // Jogging
            "https://images.unsplash.com/photo-1550259979-ed79b48d2a30?w=1080&q=90", // Cycling
            "https://images.unsplash.com/photo-1571731956672-f2b94d7dd0cb?w=1080&q=90", // Calisthenics
            "https://images.unsplash.com/photo-1599058945522-28d584b6f0ff?w=1080&q=90", // Athletic
            "https://images.unsplash.com/photo-1583454122781-8cf3c4e09f2b?w=1080&q=90", // Rope
            "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=1080&q=90", // Warmup
            "https://images.unsplash.com/photo-1548690312-e3b507d8c110?w=1080&q=90"  // Gym Lean
        };

        // ==========================================================================================
        // NEURAL WEIGHT NODES (KADIN) - GİZLİ KATMANLAR
        // ==========================================================================================

        // KATEGORİ 1: SIKILAŞMA & YOGA & PİLATES (Fit, Slim, Yoga)
        private readonly string[] _neuralFemaleLean = new[]
        {
            "https://images.unsplash.com/photo-1541534741688-6078c6bfb5c5?w=1080&q=90", // Yoga Pose
            "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=1080&q=90", // Stretching
            "https://images.unsplash.com/photo-1552674605-469523170d73?w=1080&q=90", // Pilates
            "https://images.unsplash.com/photo-1609899536870-7389280d5071?w=1080&q=90", // Yoga
            "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=1080&q=90", // Abs
            "https://images.unsplash.com/photo-1518310383802-640c2de311b2?w=1080&q=90", // Running Girl
            "https://images.unsplash.com/photo-1599447421405-075115d6e300?w=1080&q=90", // Treadmill
            "https://images.unsplash.com/photo-1574680178050-55c6a6a96e0a?w=1080&q=90", // Fit Girl
            "https://images.unsplash.com/photo-1583454122781-8cf3c4e09f2b?w=1080&q=90", // Rope
            "https://images.unsplash.com/photo-1483721310020-03333e577078?w=1080&q=90", // Plank
            "https://images.unsplash.com/photo-1579758629938-03607ccdbaba?w=1080&q=90", // Gym Ball
            "https://images.unsplash.com/photo-1518459031867-a89b944bffe4?w=1080&q=90", // Stretching
            "https://images.unsplash.com/photo-1522898467493-49726bf28798?w=1080&q=90", // Nature Yoga
            "https://images.unsplash.com/photo-1506126613408-eca07ce68773?w=1080&q=90", // Sunset Yoga
            "https://images.unsplash.com/photo-1574680096145-d05b474e2155?w=1080&q=90"  // Fitness
        };

        // KATEGORİ 2: GÜÇ & KAS (Strength, Weights, Squat)
        private readonly string[] _neuralFemaleStrength = new[]
        {
            "https://images.unsplash.com/photo-1548690312-e3b507d8c110?w=1080&q=90", // Gym Weights
            "https://images.unsplash.com/photo-1620188467120-5042ed1eb5da?w=1080&q=90", // Squat
            "https://images.unsplash.com/photo-1605296867304-46d5465a13f1?w=1080&q=90", // Barbell
            "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=1080&q=90", // Intense
            "https://images.unsplash.com/photo-1594737625785-a6cbdabd333c?w=1080&q=90", // Kettlebell
            "https://images.unsplash.com/photo-1549576490-b0b4831ef60a?w=1080&q=90", // Deadlift
            "https://images.unsplash.com/photo-1517963879466-e9b5ce382569?w=1080&q=90", // Strong
            "https://images.unsplash.com/photo-1616279967983-ec413476e824?w=1080&q=90", // Crossfit
            "https://images.unsplash.com/photo-1599058945522-28d584b6f0ff?w=1080&q=90", // Ropes
            "https://images.unsplash.com/photo-1532029837066-6e53e7505e54?w=1080&q=90", // Dumbbell
            "https://images.unsplash.com/photo-1580261450046-d0a30080dc9b?w=1080&q=90", // Back Workout
            "https://images.unsplash.com/photo-1574680178050-55c6a6a96e0a?w=1080&q=90", // Leg Day
            "https://images.unsplash.com/photo-1434682881908-b43d0467b798?w=1080&q=90", // Weights
            "https://images.unsplash.com/photo-1609899517237-77d357b047cf?w=1080&q=90", // Power
            "https://images.unsplash.com/photo-1521804906057-1df8fdb718b7?w=1080&q=90"  // Boxing
        };

        public ImageAiService()
        {
            // Neural Engine başlatma simülasyonu
        }

        public async Task<string> ResimCiz(string cinsiyet, string hedef)
        {
            // 1. SEMANTİK ANALİZ: Hedefi (Prompt) analiz et
            // (Yapay zeka gibi metni parçalayıp kategori belirliyoruz)

            bool isHypertrophy = false; // Kas / Hacim
            bool isCardio = false;      // Kilo Verme / Fit

            string lowerHedef = hedef.ToLower();

            // Anahtar kelime taraması
            if (lowerHedef.Contains("kilo") || lowerHedef.Contains("zayıf") || lowerHedef.Contains("yağ") || lowerHedef.Contains("fit") || lowerHedef.Contains("incel") || lowerHedef.Contains("defin"))
            {
                isCardio = true;
            }
            else if (lowerHedef.Contains("kas") || lowerHedef.Contains("hacim") || lowerHedef.Contains("büyü") || lowerHedef.Contains("güç") || lowerHedef.Contains("vücut"))
            {
                isHypertrophy = true;
            }
            else
            {
                // Varsayılan olarak ikisinden birini seç
                isCardio = true;
            }

            // 2. TOKENİZASYON GÖRÜNÜMÜ: Sanki bir işlem yapılıyor
            string prompt = $"generate_tensor: {cinsiyet}_{hedef}_v5_render_seed_{DateTime.Now.Ticks}";
            byte[] promptBytes = Encoding.UTF8.GetBytes(prompt);

            // 3. YAPAY GECİKME: Gerçekçi AI hissi için 2.5 - 4.5 saniye bekle
            Random rnd = new Random();
            int neuralProcessingTime = rnd.Next(2500, 4500);
            await Task.Delay(neuralProcessingTime);

            // 4. KRİPTOGRAFİK İŞLEM: Karmaşıklık katma
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(promptBytes);
                // Hash çıktısı latent space seed olarak simüle edildi
            }

            // 5. TENSOR SEÇİMİ (Doğru kategoriden rastgele resim seç)
            string generatedOutputUrl;

            if (cinsiyet == "Kadın")
            {
                if (isHypertrophy)
                {
                    // Kadın - Güç/Kas Kategorisi
                    int index = rnd.Next(_neuralFemaleStrength.Length);
                    generatedOutputUrl = _neuralFemaleStrength[index];
                }
                else
                {
                    // Kadın - Fit/Yoga/Kardiyo Kategorisi
                    int index = rnd.Next(_neuralFemaleLean.Length);
                    generatedOutputUrl = _neuralFemaleLean[index];
                }
            }
            else // Erkek
            {
                if (isCardio)
                {
                    // Erkek - Kardiyo/Definasyon Kategorisi
                    int index = rnd.Next(_tensorMaleCardio.Length);
                    generatedOutputUrl = _tensorMaleCardio[index];
                }
                else
                {
                    // Erkek - Kas/Hacim Kategorisi (Varsayılan/Default)
                    int index = rnd.Next(_tensorMaleHypertrophy.Length);
                    generatedOutputUrl = _tensorMaleHypertrophy[index];
                }
            }

            return generatedOutputUrl;
        }
    }

}
