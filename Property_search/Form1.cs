using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Property_search
{
    public partial class Form1 : Form
    {
        // Конструктор форми
        public Form1()
        {
            InitializeComponent();
            addRent.CheckedChanged += addRent_CheckedChanged;
            addProperty.SelectedIndexChanged += addProperty_SelectedIndexChanged;
            editRent.CheckedChanged += editRent_CheckedChanged;
            editProperty.SelectedIndexChanged += editProperty_SelectedIndexChanged;
            deleteRent.CheckedChanged += deleteRent_CheckedChanged;
            deleteProperty.SelectedIndexChanged += deleteProperty_SelectedIndexChanged;
            searchRent.CheckedChanged += searchRent_CheckedChanged;
            searchProperty.SelectedIndexChanged += searchProperty_SelectedIndexChanged;

        }

        // завантаження описів об’єктів нерухомості з таблиці property у ComboBox-и для редагування та видалення
        private void LoadDescriptions()
        {
            editChoseDescription.Items.Clear();
            deleteChoseDescription.Items.Clear();

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();
                string query = "SELECT description FROM property";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string desc = reader.GetString(0);
                        editChoseDescription.Items.Add(desc);
                        deleteChoseDescription.Items.Add(desc);
                    }
                }
            }
        }

        // ініціалізація даних під час завантаження форми
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDescriptions();
            LoadGridData();
            addRent_CheckedChanged(null, null);
            addProperty_SelectedIndexChanged(null, null);
            editRent_CheckedChanged(null, null);
            editProperty_SelectedIndexChanged(null, null);
            LoadGridData();
        }

        // кнопка очищення в групі додавання
        private void addClean_Click(object sender, EventArgs e)
        {
            addDescription.Text = "";
            addOwner.Text = "";
            addNumber.Text = "";
            addRent.Checked = false;
            addSale.Checked = false;
            addProperty.SelectedIndex = -1;
            addDistrict.SelectedIndex = -1;
            addRoom.SelectedIndex = -1;
            addPrice.Text = "";
            addFloor.Text = "";
            addLivingArea.Text = "";
            addLandArea.Text = "";
            addLandArea.Visible = false;
            addPetsAllowed.Checked = false;
            addPetsAllowed.Visible = false;
            label25.Visible = false;

        }

        // кнопка очищення в групі редагування
        private void editClean_Click(object sender, EventArgs e)
        {
            editChoseDescription.SelectedIndex = -1;
            editID.Text = "";
            editDescription.Text = "";
            editOwner.Text = "";
            editNumber.Text = "";
            editRent.Checked = false;
            editSale.Checked = false;
            editProperty.SelectedIndex = -1;
            editDistrict.SelectedIndex = -1;
            editRoom.SelectedIndex = -1;
            editStatus.SelectedIndex = -1;
            editStatus.Visible = false;
            editPrice.Text = "";
            editFloor.Text = "";
            editLivingArea.Text = "";
            editLandArea.Text = "";
            editLandArea.Visible = false;
            editPetsAllowed.Checked = false;
            editPetsAllowed.Visible = false;
            label29.Visible = false;
        }

        // кнопка очищення в групі видалення
        private void deleteClean_Click(object sender, EventArgs e)
        {
            deleteChoseDescription.SelectedIndex = -1;
            deleteID.Text = "";
            deleteDescription.Text = "";
            deleteOwner.Text = "";
            deleteNumber.Text = "";
            deleteRent.Checked = false;
            deleteSale.Checked = false;
            deleteProperty.SelectedIndex = -1;
            deleteDistrict.SelectedIndex = -1;
            deleteRoom.SelectedIndex = -1;
            deleteStatus.SelectedIndex = -1;
            deleteStatus.Visible = false;
            deletePrice.Text = "";
            deleteFloor.Text = "";
            deleteLivingArea.Text = "";
            deleteLandArea.Text = "";
            deleteLandArea.Visible = false;
            deletePetsAllowed.Checked = false;
            deletePetsAllowed.Visible = false;
            label42.Visible = false;
        }

        // кнопка очищення в групі пошуку за фільтрами
        private void searchClean_Click(object sender, EventArgs e)
        {
            searchRent.Checked = false;
            searchSale.Checked = false;
            searchProperty.SelectedIndex = -1;
            searchDistrict.SelectedIndex = -1;
            searchRoom.SelectedIndex = -1;
            searchPetsAllowed.Checked = false;
            searchPetsAllowed.Visible = false;
            searchPriceFrom.Text = "";
            searchPriceTo.Text = "";
            searchFloorFrom.Text = "";
            searchFloorTo.Text = "";
            searchLivingAreaFrom.Text = "";
            searchLivingAreaTo.Text = "";
            searchLandAreaFrom.Text = "";
            searchLandAreaTo.Text = "";
            dataGridView1.DataSource = null;
            label16.Visible = false;
            label15.Visible = false;
            label14.Visible = false;
            searchLandAreaFrom.Visible = false;
            searchLandAreaTo.Visible = false;
        }

        //перевірка правильності формату номера телефону
        private bool validPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+\d{2,3}\s\d{2}\s\d{3}\s\d{4}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }


        //метод для перевірки чи всі поля заповнені для групи додавання
        private bool areAddFieldsFilled()
        {
            // основні поля
            if (string.IsNullOrWhiteSpace(addDescription.Text) || string.IsNullOrWhiteSpace(addOwner.Text) ||
                string.IsNullOrWhiteSpace(addNumber.Text) || (!addRent.Checked && !addSale.Checked) ||
                addProperty.SelectedIndex == -1 || addDistrict.SelectedIndex == -1 || addRoom.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(addPrice.Text) || string.IsNullOrWhiteSpace(addFloor.Text) ||
                string.IsNullOrWhiteSpace(addLivingArea.Text))
                return false;
            // додаткове поле з дялінкою коли обрано будинок
            if (addProperty.SelectedItem?.ToString() == "Будинок" &&
                string.IsNullOrWhiteSpace(addLandArea.Text))
                return false;
            // додаткове поле з тваринами коли об'єкт для оренди
            if (addRent.Checked && !addPetsAllowed.Visible)
                return false;

            return true;
        }

        //перевірка на заповненість полів для групи редагування
        private bool areEditFieldsFilled()
        {
            if (string.IsNullOrWhiteSpace(editDescription.Text) || string.IsNullOrWhiteSpace(editOwner.Text) ||
                string.IsNullOrWhiteSpace(editNumber.Text) || (!editRent.Checked && !editSale.Checked) ||
                editProperty.SelectedIndex == -1 || editDistrict.SelectedIndex == -1 || editRoom.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(editPrice.Text) || string.IsNullOrWhiteSpace(editFloor.Text) ||
                string.IsNullOrWhiteSpace(editLivingArea.Text))
                return false;

            if (editProperty.SelectedItem?.ToString() == "Будинок" && string.IsNullOrWhiteSpace(editLandArea.Text))
                return false;

            if (editRent.Checked && (!editPetsAllowed.Visible || editStatus.SelectedIndex == -1))
                return false;

            return true;
        }

        // видимість необов'язкових полів
        private void SetVisibility(bool visible, params Control[] controls)
        {
            foreach (var ctrl in controls) ctrl.Visible = visible;
        }

        // видимість поля тварин в групі додавання запису до бд
        private void addRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(addRent.Checked, addPetsAllowed);
        }

        // видимість полів з площею ділянки та надписом в групі додавання запису до бд
        private void addProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addProperty.SelectedItem != null)
            {
                bool isHouse = addProperty.SelectedItem.ToString() == "Будинок";
                SetVisibility(isHouse, addLandArea, label25);
            }
        }

        // видимість поля тварин в групі редагування запису
        private void editRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(editRent.Checked, editPetsAllowed, label23, editStatus);
        }

        // видимість полів з площею ділянки та надписом в групі редагування запису
        private void editProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editProperty.SelectedItem != null)
            {
                bool isHouse = editProperty.SelectedItem.ToString() == "Будинок";
                SetVisibility(isHouse, editLandArea, label29);
            }
        }

        // видимість поля тварин в групі видалення запису
        private void deleteRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(deleteRent.Checked, deletePetsAllowed, label49, deleteStatus);
        }

        // видимість полів з площею ділянки та надписом в групі видалення запису
        private void deleteProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deleteProperty.SelectedItem != null)
            {
                bool isHouse = deleteProperty.SelectedItem.ToString() == "Будинок";
                SetVisibility(isHouse, deleteLandArea, label42);
            }
        }

        // видимість поля тварин для пошуку записів
        private void searchRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(searchRent.Checked, searchPetsAllowed);
        }

        // видимість полів з площею ділянки та надписом в групі пошуку записів
        private void searchProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchProperty.SelectedItem != null)
            {
                bool isHouse = searchProperty.SelectedItem.ToString() == "Будинок";
                SetVisibility(isHouse, searchLandAreaFrom, searchLandAreaTo, label14, label15, label16);
            }
        }

        // перевірка полів на цілі числа
        private bool AreAllInt(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {

                if (textBox == null || !textBox.Visible || string.IsNullOrWhiteSpace(textBox.Text))
                    continue;

                if (!int.TryParse(textBox.Text, out _))
                {
                    MessageBox.Show($"Поле \"{textBox.Name}\" повинно містити ціле число.");
                    textBox.Focus();
                    return false;
                }
            }
            return true;
        }

        // перевірка полів на дійсні числа з не більше ніж 2 знаками після коми
        private bool AreAllDecimals(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                if (textBox == null || !textBox.Visible || string.IsNullOrWhiteSpace(textBox.Text))
                    continue;

                if (decimal.TryParse(textBox.Text, out decimal result))
                {
                    if (Decimal.Round(result, 2) != result)
                    {
                        string fieldName = textBox.Tag?.ToString() ?? textBox.Name;
                        MessageBox.Show($"Поле \"{fieldName}\" повинно містити число з не більше ніж двома знаками після коми.");
                        textBox.Focus();
                        return false;
                    }
                }
                else
                {
                    string fieldName = textBox.Tag?.ToString() ?? textBox.Name;
                    MessageBox.Show($"Поле \"{fieldName}\" повинно містити дійсне число.");
                    textBox.Focus();
                    return false;
                }
            }
            return true;
        }

        // кнопка для додавання запису до бази даних
        private void AddToDatabase_Click(object sender, EventArgs e)
        {

            if (!areAddFieldsFilled())
            {
                MessageBox.Show("Будь ласка, заповніть всі поля!");
                return;
            }

            if (!AreAllInt(addFloor))
                return;

            if (!AreAllDecimals(addPrice, addLivingArea, addLandArea.Visible ? addLandArea : null))
                return;

            if (!validPhoneNumber(addNumber.Text))
            {
                MessageBox.Show("Перевірте правильність формату номера телефону. \nПотрібний формат: +ххх (або +хх) хх ххх хххх");
                return;
            }

            // перевірка унікальності опису
            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM property WHERE description = @desc";
                using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", addDescription.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Об'єкт із таким описом вже існує!\n Якщо хочете додати новий об'єкт, змініть опис на унікальний.\n Якщо потрібно внести зміни до вже існуючого об'єкта, то перейдіть в групу \"Редагувати інформацію\".");
                        return;
                    }
                }

                // додавання в основну таблицю
                string insertProperty = @"INSERT INTO property 
                (description, owner, phone_number, operation_type, property_type, district, number_of_rooms, price, floor, living_area) 
                VALUES (@desc, @owner, @phone, @optype, @ptype, @district, @rooms, @price, @floor, @living)";

                using (MySqlCommand cmd = new MySqlCommand(insertProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", addDescription.Text);
                    cmd.Parameters.AddWithValue("@owner", addOwner.Text);
                    cmd.Parameters.AddWithValue("@phone", addNumber.Text);
                    cmd.Parameters.AddWithValue("@optype", addRent.Checked ? "Оренда" : "Продаж");
                    cmd.Parameters.AddWithValue("@ptype", addProperty.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@district", addDistrict.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(addRoom.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(addPrice.Text));
                    cmd.Parameters.AddWithValue("@floor", int.Parse(addFloor.Text));
                    cmd.Parameters.AddWithValue("@living", decimal.Parse(addLivingArea.Text));
                    cmd.ExecuteNonQuery();
                }

                // отримання ID цього запису
                object result = new MySqlCommand("SELECT LAST_INSERT_ID()", conn).ExecuteScalar();
                long newID = Convert.ToInt64(result);

                // додавання площі дялянки для будинку в таблицю house_details
                if (addProperty.SelectedItem.ToString() == "Будинок")
                {
                    string insertHouse = "INSERT INTO house_details (id_property, land_area) VALUES (@id, @land)";
                    using (MySqlCommand cmd = new MySqlCommand(insertHouse, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", newID);
                        cmd.Parameters.AddWithValue("@land", decimal.Parse(addLandArea.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                // додавання статусу і тварин для оренди в rental_details
                if (addRent.Checked)
                {
                    string insertRent = "INSERT INTO rental_details (id_property, pets_allow, apartment_status) VALUES (@id, @pets, 'Здається')";
                    using (MySqlCommand cmd = new MySqlCommand(insertRent, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", newID);
                        cmd.Parameters.AddWithValue("@pets", addPetsAllowed.Checked ? 1 : 0);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Об'єкт успішно додано до бази даних!");
                addClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // вивід всіх даних для обраного опису нерухомості в групу редагування
        private void editChoseDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDesc = editChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc)) return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // основні поля
                string query = "SELECT * FROM property WHERE description = @desc";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", selectedDesc);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            editID.Text = reader["id_property"].ToString();
                            editDescription.Text = reader["description"].ToString();
                            editOwner.Text = reader["owner"].ToString();
                            editNumber.Text = reader["phone_number"].ToString();
                            string operation = reader["operation_type"].ToString();
                            editRent.Checked = operation == "Оренда";
                            editSale.Checked = operation == "Продаж";
                            editProperty.SelectedItem = reader["property_type"].ToString();
                            editDistrict.SelectedItem = reader["district"].ToString();
                            editRoom.SelectedItem = reader["number_of_rooms"].ToString();
                            editPrice.Text = reader["price"].ToString();
                            editFloor.Text = reader["floor"].ToString();
                            editLivingArea.Text = reader["living_area"].ToString();

                            editRent_CheckedChanged(null, null);
                            editProperty_SelectedIndexChanged(null, null);
                        }
                    }
                }

                // додаткове поле з дялінкою коли обрано будинок
                string rentQ = "SELECT pets_allow, apartment_status FROM rental_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(rentQ, conn))
                {
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            editPetsAllowed.Checked = reader.GetInt32(0) == 1;
                            editStatus.SelectedItem = reader.GetString(1);
                            editStatus.Visible = true;
                            editPetsAllowed.Visible = true;
                        }
                        else
                        {
                            editStatus.Visible = false;
                            editPetsAllowed.Visible = false;
                        }
                    }
                }

                // додаткове поле з тваринами коли об'єкт для оренди
                string houseQ = "SELECT land_area FROM house_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(houseQ, conn))
                {
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    object land = cmd.ExecuteScalar();
                    if (land != null)
                    {
                        editLandArea.Text = land.ToString();
                        editLandArea.Visible = true;
                    }
                    else
                    {
                        editLandArea.Visible = false;
                    }
                }
            }
        }

        // кнопка для збереження змін про запис в базі даних
        private void Update_Click(object sender, EventArgs e)
        {
            if (!areEditFieldsFilled())
            {
                MessageBox.Show("Будь ласка, заповніть всі поля!");
                return;
            }

            if (!AreAllInt(addFloor))
                return;

            if (!AreAllDecimals(addPrice, addLivingArea, addLandArea.Visible ? addLandArea : null))
                return;

            if (!validPhoneNumber(editNumber.Text))
            {
                MessageBox.Show("Перевірте правильність формату номера телефону. \nПотрібний формат: +ххх (або +хх) хх ххх хххх");
                return;
            }

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // перевірка унікальності опису
                string checkQuery = "SELECT COUNT(*) FROM property WHERE description = @desc AND id_property != @id";
                using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", editDescription.Text);
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Об'єкт із таким описом вже існує!");
                        return;
                    }
                }

                // оновлення основної таблиці
                string updateProperty = @"UPDATE property SET description = @desc, owner = @owner, phone_number = @phone, operation_type = @optype, 
                property_type = @ptype, district = @district, number_of_rooms = @rooms, price = @price, floor = @floor, living_area = @living
                WHERE id_property = @id";

                using (MySqlCommand cmd = new MySqlCommand(updateProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    cmd.Parameters.AddWithValue("@desc", editDescription.Text);
                    cmd.Parameters.AddWithValue("@owner", editOwner.Text);
                    cmd.Parameters.AddWithValue("@phone", editNumber.Text);
                    cmd.Parameters.AddWithValue("@optype", editRent.Checked ? "Оренда" : "Продаж");
                    cmd.Parameters.AddWithValue("@ptype", editProperty.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@district", editDistrict.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(editRoom.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(editPrice.Text));
                    cmd.Parameters.AddWithValue("@floor", int.Parse(editFloor.Text));
                    cmd.Parameters.AddWithValue("@living", decimal.Parse(editLivingArea.Text));
                    cmd.ExecuteNonQuery();

                    // видаляємо запис з house_details якщо тип вже не будинок
                    if (editProperty.SelectedItem.ToString() != "Будинок")
                    {
                        string deleteHouse = "DELETE FROM house_details WHERE id_property = @id";
                        using (MySqlCommand cmdDel = new MySqlCommand(deleteHouse, conn))
                        {
                            cmdDel.Parameters.AddWithValue("@id", editID.Text);
                            cmdDel.ExecuteNonQuery();
                        }
                    }

                    // видаляємо запис з rental_details якщо об'єкт вже не для оренди
                    if (!editRent.Checked)
                    {
                        string deleteRent = "DELETE FROM rental_details WHERE id_property = @id";
                        using (MySqlCommand cmdDel = new MySqlCommand(deleteRent, conn))
                        {
                            cmdDel.Parameters.AddWithValue("@id", editID.Text);
                            cmdDel.ExecuteNonQuery();
                        }
                    }
                }

                // оновлення для площі ділянки якщо обрано будинок
                if (editProperty.SelectedItem.ToString() == "Будинок")
                {
                    string houseQuery = "REPLACE INTO house_details (id_property, land_area) VALUES (@id, @land)";
                    using (MySqlCommand cmd = new MySqlCommand(houseQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", editID.Text);
                        cmd.Parameters.AddWithValue("@land", decimal.Parse(editLandArea.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                // оновлення для орендних даних
                if (editRent.Checked)
                {
                    string rentQuery = "REPLACE INTO rental_details (id_property, pets_allow, apartment_status) VALUES (@id, @pets, @status)";
                    using (MySqlCommand cmd = new MySqlCommand(rentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", editID.Text);
                        cmd.Parameters.AddWithValue("@pets", editPetsAllowed.Checked ? 1 : 0);
                        cmd.Parameters.AddWithValue("@status", editStatus.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Інформацію успішно оновлено!");
                editClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // вивід всіх даних для обраного опису нерухомості в групу видалення
        private void deleteChoseDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDesc = deleteChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc)) return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // основні поля
                string query = "SELECT * FROM property WHERE description = @desc";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", selectedDesc);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deleteID.Text = reader["id_property"].ToString();
                            deleteDescription.Text = reader["description"].ToString();
                            deleteOwner.Text = reader["owner"].ToString();
                            deleteNumber.Text = reader["phone_number"].ToString();

                            string operation = reader["operation_type"].ToString();
                            deleteRent.Checked = operation == "Оренда";
                            deleteSale.Checked = operation == "Продаж";

                            deleteProperty.SelectedItem = reader["property_type"].ToString();
                            deleteDistrict.SelectedItem = reader["district"].ToString();
                            deleteRoom.SelectedItem = reader["number_of_rooms"].ToString();

                            deletePrice.Text = reader["price"].ToString();
                            deleteFloor.Text = reader["floor"].ToString();
                            deleteLivingArea.Text = reader["living_area"].ToString();

                            deleteRent_CheckedChanged(null, null);
                            deleteProperty_SelectedIndexChanged(null, null);
                        }
                    }
                }

                // додаткові дані для оренди
                string rentQ = "SELECT pets_allow, apartment_status FROM rental_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(rentQ, conn))
                {
                    cmd.Parameters.AddWithValue("@id", deleteID.Text);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deletePetsAllowed.Checked = reader.GetInt32(0) == 1;
                            deleteStatus.SelectedItem = reader.GetString(1);
                            deletePetsAllowed.Visible = true;
                            deleteStatus.Visible = true;
                        }
                        else
                        {
                            deletePetsAllowed.Visible = false;
                            deleteStatus.Visible = false;
                        }
                    }
                }

                // додаткові дані для будинку
                string houseQ = "SELECT land_area FROM house_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(houseQ, conn))
                {
                    cmd.Parameters.AddWithValue("@id", deleteID.Text);
                    object land = cmd.ExecuteScalar();
                    if (land != null)
                    {
                        deleteLandArea.Text = land.ToString();
                        deleteLandArea.Visible = true;
                    }
                    else
                    {
                        deleteLandArea.Visible = false;
                    }
                }
            }
        }

        // кнопка для видалення об'єкту
        private void Delete_Click(object sender, EventArgs e)
        {
            string selectedDesc = deleteChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc))
            {
                MessageBox.Show("Оберіть об'єкт для видалення зі спадного списку!");
                return;
            }
            
            // підтвердження видалення
            var confirmResult = MessageBox.Show($"Ви впевнені, що хочете видалити об'єкт: \"{selectedDesc}\"?", "Підтвердити видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult != DialogResult.Yes)
                return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // беремо id за описом
                int idToDelete = 0;
                string getIdQuery = "SELECT id_property FROM property WHERE description = @desc";
                using (MySqlCommand cmd = new MySqlCommand(getIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", selectedDesc);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Об'єкт не знайдено! Оберіть потрібний опис зі спадного списку!");
                        return;
                    }
                    idToDelete = Convert.ToInt32(result);
                }

                // видаляємо рядок з rental_details
                string deleteRent = "DELETE FROM rental_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteRent, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                // видаляємо рядок з house_details
                string deleteHouse = "DELETE FROM house_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteHouse, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                // видаляємо рядок з property
                string deleteProperty = "DELETE FROM property WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Об'єкт успішно видалено!");
                deleteClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // вивід всіх записів у dataGridView2 на вкладці "Керування об'єктами"
        private void LoadGridData()
        {
            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                string query = @"SELECT p.id_property, p.description, p.owner, p.phone_number, p.operation_type, p.property_type, 
                p.district, p.number_of_rooms, p.price, p.floor, p.living_area, r.pets_allow, r.apartment_status, h.land_area
                FROM property p
                LEFT JOIN rental_details r ON p.id_property = r.id_property
                LEFT JOIN house_details h ON p.id_property = h.id_property";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView2.DataSource = table;
                }
            }

            dataGridView2.Columns["id_property"].HeaderText = "ID";
            dataGridView2.Columns["description"].HeaderText = "Опис";
            dataGridView2.Columns["owner"].HeaderText = "Власник";
            dataGridView2.Columns["phone_number"].HeaderText = "Телефон";
            dataGridView2.Columns["operation_type"].HeaderText = "Операція";
            dataGridView2.Columns["property_type"].HeaderText = "Тип";
            dataGridView2.Columns["district"].HeaderText = "Район";
            dataGridView2.Columns["number_of_rooms"].HeaderText = "Кімнати";
            dataGridView2.Columns["price"].HeaderText = "Ціна";
            dataGridView2.Columns["floor"].HeaderText = "Поверх";
            dataGridView2.Columns["living_area"].HeaderText = "Житлова площа";
            dataGridView2.Columns["pets_allow"].HeaderText = "Дозволено тварин";
            dataGridView2.Columns["apartment_status"].HeaderText = "Статус квартири";
            dataGridView2.Columns["land_area"].HeaderText = "Площа ділянки";
        }

        // перевірка правильності діапазону в полях "від" та "до"
        private bool ValidateRange(TextBox fromBox, TextBox toBox, string fieldName)
        {
            int from = 0, to = 0;

            // перевірка чи це числа
            if (!string.IsNullOrWhiteSpace(fromBox.Text) && !int.TryParse(fromBox.Text, out from))
            {
                MessageBox.Show($"В рядку '{fieldName}' значення 'від' має бути числовим.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(toBox.Text) && !int.TryParse(toBox.Text, out to))
            {
                MessageBox.Show($"В рядку '{fieldName}' значення 'до' має бути числовим.");
                return false;
            }

            // перевірка діапазону
            if (!string.IsNullOrWhiteSpace(fromBox.Text) && !string.IsNullOrWhiteSpace(toBox.Text))
            {
                if (from > to)
                {
                    MessageBox.Show($"В рядку '{fieldName}' значення 'від' повинно бути меншим за 'до'.");
                    return false;
                }
            }

            return true;
        }

        // кнопка пошуку записів і виведення записів з бази даних за обраними фільтрами
        private void Search_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {

                if (!ValidateRange(searchPriceFrom, searchPriceTo, "Ціна") || 
                    !ValidateRange(searchFloorFrom, searchFloorTo, "Поверх") ||
                    !ValidateRange(searchLivingAreaFrom, searchLivingAreaTo, "Житлова площа") || 
                    !ValidateRange(searchLandAreaFrom, searchLandAreaTo, "Площа ділянки"))
                {
                    return;
                }

                conn.Open();
                List<string> conditions = new List<string>();
                MySqlCommand cmd = new MySqlCommand();
                string baseQuery = @"SELECT p.id_property, p.description, p.owner, p.phone_number, p.operation_type, p.property_type, p.district, p.number_of_rooms, 
                p.price, p.floor, p.living_area, r.pets_allow, r.apartment_status, h.land_area
                FROM property p
                LEFT JOIN rental_details r ON p.id_property = r.id_property
                LEFT JOIN house_details h ON p.id_property = h.id_property";

                // тиип операції
                if (searchRent.Checked)
                {
                    conditions.Add("p.operation_type = 'Оренда'");
                    conditions.Add("r.apartment_status = 'Здається'");
                    if (searchPetsAllowed.Checked)
                        conditions.Add("r.pets_allow = 1");
                    else
                        conditions.Add("r.pets_allow = 0");
                }
                else if (searchSale.Checked)
                {
                    conditions.Add("p.operation_type = 'Продаж'");
                }

                // тип нерухомості
                if (searchProperty.SelectedIndex != -1)
                {
                    conditions.Add("p.property_type = @ptype");
                    cmd.Parameters.AddWithValue("@ptype", searchProperty.SelectedItem.ToString());
                }

                // район
                if (searchDistrict.SelectedIndex != -1)
                {
                    conditions.Add("p.district = @district");
                    cmd.Parameters.AddWithValue("@district", searchDistrict.SelectedItem.ToString());
                }

                // кількість кімнат
                if (searchRoom.SelectedIndex != -1)
                {
                    conditions.Add("p.number_of_rooms = @rooms");
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(searchRoom.SelectedItem.ToString()));
                }

                // з тваринами
                if (searchPetsAllowed.Checked)
                {
                    conditions.Add("r.pets_allow = 1");
                }

                // діапазон з ціною, поверхом, житловою площею й ділянкою
                AddRangeFilter("p.price", searchPriceFrom, searchPriceTo, cmd, conditions);
                AddRangeFilter("p.floor", searchFloorFrom, searchFloorTo, cmd, conditions);
                AddRangeFilter("p.living_area", searchLivingAreaFrom, searchLivingAreaTo, cmd, conditions);
                AddRangeFilter("h.land_area", searchLandAreaFrom, searchLandAreaTo, cmd, conditions);

                // динамічне задання введених умов
                if (conditions.Count > 0)
                    baseQuery += " WHERE " + string.Join(" AND ", conditions);

                cmd.CommandText = baseQuery;
                cmd.Connection = conn;

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    dataGridView1.Columns["description"].HeaderText = "Опис";
                    dataGridView1.Columns["owner"].HeaderText = "Власник";
                    dataGridView1.Columns["phone_number"].HeaderText = "Телефон";
                    dataGridView1.Columns["operation_type"].HeaderText = "Операція";
                    dataGridView1.Columns["property_type"].HeaderText = "Тип";
                    dataGridView1.Columns["district"].HeaderText = "Район";
                    dataGridView1.Columns["number_of_rooms"].HeaderText = "Кімнати";
                    dataGridView1.Columns["price"].HeaderText = "Ціна";
                    dataGridView1.Columns["floor"].HeaderText = "Поверх";
                    dataGridView1.Columns["living_area"].HeaderText = "Житлова площа";
                    dataGridView1.Columns["pets_allow"].HeaderText = "Дозволено тварин";
                    dataGridView1.Columns["apartment_status"].HeaderText = "Статус квартири";
                    dataGridView1.Columns["land_area"].HeaderText = "Площа ділянки";

                }
            }
        }

        // додавання умов для діапазону значень у WHERE запит
        private void AddRangeFilter(string field, TextBox fromBox, TextBox toBox, MySqlCommand cmd, List<string> conditions)
        {
            if (!string.IsNullOrWhiteSpace(fromBox.Text))
            {
                string param = "@from_" + field.Replace(".", "_");
                conditions.Add($"{field} >= {param}");
                cmd.Parameters.AddWithValue(param, decimal.Parse(fromBox.Text));
            }

            if (!string.IsNullOrWhiteSpace(toBox.Text))
            {
                string param = "@to_" + field.Replace(".", "_");
                conditions.Add($"{field} <= {param}");
                cmd.Parameters.AddWithValue(param, decimal.Parse(toBox.Text));
            }
        }
    }
}
