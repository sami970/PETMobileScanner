
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PETScanner.Model;

namespace PETScanner
{
    public class JsonFileService
    {
        private string fileName = "userdata.json";

        public async Task SaveDataAsync(UserInputData data)
        {
            string json = JsonSerializer.Serialize(data);
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using FileStream fs = File.Create(filePath);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await fs.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task<UserInputData> LoadUserDataAsync()
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
                return null;

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<UserInputData>(json);
        }
    }
}
