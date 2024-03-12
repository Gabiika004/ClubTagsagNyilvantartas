using ClubTagsagNyilvantartas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClubTagsagGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _client = new HttpClient();

        public ApiService()
        {
            _client.BaseAddress = new Uri("https://retoolapi.dev/OEDUXm/member");
        }

        // Olvasás (Read)
        public async Task<Member[]> GetAllMembersAsync()
        {
            try
            {
                var response = await _client.GetAsync("");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return Member.FromJson(content);
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Hiba történt a kéréskor: {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Hiba történt a tagok lekérésekor: {e.Message}");
                return null;
            }
        }

        // Létrehozás (Create)
        public async Task<Member> CreateMemberAsync(Member newMember)
        {
            try
            {
                var json = JsonConvert.SerializeObject(newMember);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Member>(responseContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Hiba történt egy tag létrehozásakor: {e.Message}");
                return null;
            }
        }

        // Frissítés (Update)
        public async Task<Member> UpdateMemberAsync(long id, Member memberToUpdate)
        {
            var json = JsonConvert.SerializeObject(memberToUpdate, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{_client.BaseAddress}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var updatedMember = JsonConvert.DeserializeObject<Member>(responseContent);
                return updatedMember;
            }
            else
            {
                throw new Exception($"Nem sikerült frissíteni a tagot. HTTP Status: {response.StatusCode}");
            }
        }


        // Törlés (Delete)
        public async Task<bool> DeleteMemberAsync(long id)
        {
            var url = $"{_client.BaseAddress}/{id}";
            System.Diagnostics.Debug.WriteLine($"Törlési kérés küldése: {url}");

            try
            {
                var response = await _client.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync(); 

                // A válasz sikerességének ellenőrzése
                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("A törlés sikeres volt.");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"A törlés sikertelen. HTTP státusz: {response.StatusCode}, válasz: {content}");
                    MessageBox.Show($"Nem sikerült törölni a tagot. Hiba: {content}", "Törlési Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Kivétel történt a törlés során: {e.Message}");
                MessageBox.Show($"Hiba történt egy tag törlésekor: {e.Message}", "Törlési Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
}
