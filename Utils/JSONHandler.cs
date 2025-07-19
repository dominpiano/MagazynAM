using MagazynAM.MVVM.Model;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace MagazynAM.Utils {
    public class JSONHandler {

        private string jsonPath;
        private string jsonText;
        private List<StoreItem> storeItems;

        public JSONHandler() {
            jsonPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, @"Files\items.json");
        }

        //Main methods
        public void AddItemToJSON(StoreItem itemToAdd) {
            var items = LoadItemsFromJSON();
            itemToAdd.ID = items.Last().ID + 1;
            items.Add(itemToAdd);
            SaveToJSON(items);
        }

        public void RemoveItemFromJSON(StoreItem itemToRemove) {
            var items = LoadItemsFromJSON();
            items.RemoveAt(items.FindIndex(i => i.ID == itemToRemove.ID));
            SaveToJSON(items);
        }

        public void EditItemInJSON(StoreItem itemToEdit) {
            var items = LoadItemsFromJSON();
            var index = items.FindIndex(i => i.ID == itemToEdit.ID);
            items[index] = itemToEdit;
            SaveToJSON(items);
        }

        public List<StoreItem> LoadItemsFromJSON() {
            try {

                jsonText = File.ReadAllText(jsonPath);
                if (!string.IsNullOrWhiteSpace(jsonText)) {
                    var deserialized = JsonConvert.DeserializeObject<List<StoreItem>>(jsonText);

                    if (deserialized != null)
                        storeItems = deserialized;
                }
            }
            catch (JsonException ex) {
                MessageBox.Show("JSON Parse error: " + ex.Message);
                Application.Current.Shutdown();
            }
            catch (Exception ex) {
                MessageBox.Show("Unknown exception: " + ex.Message);
                Application.Current.Shutdown();
            }

            return storeItems;
        }

        private void SaveToJSON(List<StoreItem> items) {
            var serialized = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(jsonPath, serialized);
        }
    }
}
