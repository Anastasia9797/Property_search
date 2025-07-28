using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Property_search
{
    public partial class Form1 : Form
    {
        // ����������� �����
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

        // ������������ ����� �ᒺ��� ���������� � ������� property � ComboBox-� ��� ����������� �� ���������
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

        // ����������� ����� �� ��� ������������ �����
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

        // ������ �������� � ���� ���������
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

        // ������ �������� � ���� �����������
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

        // ������ �������� � ���� ���������
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

        // ������ �������� � ���� ������ �� ���������
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

        //�������� ����������� ������� ������ ��������
        private bool validPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+\d{2,3}\s\d{2}\s\d{3}\s\d{4}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }


        //����� ��� �������� �� �� ���� �������� ��� ����� ���������
        private bool areAddFieldsFilled()
        {
            // ������ ����
            if (string.IsNullOrWhiteSpace(addDescription.Text) || string.IsNullOrWhiteSpace(addOwner.Text) ||
                string.IsNullOrWhiteSpace(addNumber.Text) || (!addRent.Checked && !addSale.Checked) ||
                addProperty.SelectedIndex == -1 || addDistrict.SelectedIndex == -1 || addRoom.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(addPrice.Text) || string.IsNullOrWhiteSpace(addFloor.Text) ||
                string.IsNullOrWhiteSpace(addLivingArea.Text))
                return false;
            // ��������� ���� � ������� ���� ������ �������
            if (addProperty.SelectedItem?.ToString() == "�������" &&
                string.IsNullOrWhiteSpace(addLandArea.Text))
                return false;
            // ��������� ���� � ��������� ���� ��'��� ��� ������
            if (addRent.Checked && !addPetsAllowed.Visible)
                return false;

            return true;
        }

        //�������� �� ����������� ���� ��� ����� �����������
        private bool areEditFieldsFilled()
        {
            if (string.IsNullOrWhiteSpace(editDescription.Text) || string.IsNullOrWhiteSpace(editOwner.Text) ||
                string.IsNullOrWhiteSpace(editNumber.Text) || (!editRent.Checked && !editSale.Checked) ||
                editProperty.SelectedIndex == -1 || editDistrict.SelectedIndex == -1 || editRoom.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(editPrice.Text) || string.IsNullOrWhiteSpace(editFloor.Text) ||
                string.IsNullOrWhiteSpace(editLivingArea.Text))
                return false;

            if (editProperty.SelectedItem?.ToString() == "�������" && string.IsNullOrWhiteSpace(editLandArea.Text))
                return false;

            if (editRent.Checked && (!editPetsAllowed.Visible || editStatus.SelectedIndex == -1))
                return false;

            return true;
        }

        // �������� ������'������� ����
        private void SetVisibility(bool visible, params Control[] controls)
        {
            foreach (var ctrl in controls) ctrl.Visible = visible;
        }

        // �������� ���� ������ � ���� ��������� ������ �� ��
        private void addRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(addRent.Checked, addPetsAllowed);
        }

        // �������� ���� � ������ ������ �� �������� � ���� ��������� ������ �� ��
        private void addProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addProperty.SelectedItem != null)
            {
                bool isHouse = addProperty.SelectedItem.ToString() == "�������";
                SetVisibility(isHouse, addLandArea, label25);
            }
        }

        // �������� ���� ������ � ���� ����������� ������
        private void editRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(editRent.Checked, editPetsAllowed, label23, editStatus);
        }

        // �������� ���� � ������ ������ �� �������� � ���� ����������� ������
        private void editProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editProperty.SelectedItem != null)
            {
                bool isHouse = editProperty.SelectedItem.ToString() == "�������";
                SetVisibility(isHouse, editLandArea, label29);
            }
        }

        // �������� ���� ������ � ���� ��������� ������
        private void deleteRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(deleteRent.Checked, deletePetsAllowed, label49, deleteStatus);
        }

        // �������� ���� � ������ ������ �� �������� � ���� ��������� ������
        private void deleteProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deleteProperty.SelectedItem != null)
            {
                bool isHouse = deleteProperty.SelectedItem.ToString() == "�������";
                SetVisibility(isHouse, deleteLandArea, label42);
            }
        }

        // �������� ���� ������ ��� ������ ������
        private void searchRent_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(searchRent.Checked, searchPetsAllowed);
        }

        // �������� ���� � ������ ������ �� �������� � ���� ������ ������
        private void searchProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchProperty.SelectedItem != null)
            {
                bool isHouse = searchProperty.SelectedItem.ToString() == "�������";
                SetVisibility(isHouse, searchLandAreaFrom, searchLandAreaTo, label14, label15, label16);
            }
        }

        // �������� ���� �� ��� �����
        private bool AreAllInt(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {

                if (textBox == null || !textBox.Visible || string.IsNullOrWhiteSpace(textBox.Text))
                    continue;

                if (!int.TryParse(textBox.Text, out _))
                {
                    MessageBox.Show($"���� \"{textBox.Name}\" ������� ������ ���� �����.");
                    textBox.Focus();
                    return false;
                }
            }
            return true;
        }

        // �������� ���� �� ���� ����� � �� ����� �� 2 ������� ���� ����
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
                        MessageBox.Show($"���� \"{fieldName}\" ������� ������ ����� � �� ����� �� ����� ������� ���� ����.");
                        textBox.Focus();
                        return false;
                    }
                }
                else
                {
                    string fieldName = textBox.Tag?.ToString() ?? textBox.Name;
                    MessageBox.Show($"���� \"{fieldName}\" ������� ������ ����� �����.");
                    textBox.Focus();
                    return false;
                }
            }
            return true;
        }

        // ������ ��� ��������� ������ �� ���� �����
        private void AddToDatabase_Click(object sender, EventArgs e)
        {

            if (!areAddFieldsFilled())
            {
                MessageBox.Show("���� �����, �������� �� ����!");
                return;
            }

            if (!AreAllInt(addFloor))
                return;

            if (!AreAllDecimals(addPrice, addLivingArea, addLandArea.Visible ? addLandArea : null))
                return;

            if (!validPhoneNumber(addNumber.Text))
            {
                MessageBox.Show("�������� ����������� ������� ������ ��������. \n�������� ������: +��� (��� +��) �� ��� ����");
                return;
            }

            // �������� ���������� �����
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
                        MessageBox.Show("��'��� �� ����� ������ ��� ����!\n ���� ������ ������ ����� ��'���, ����� ���� �� ���������.\n ���� ������� ������ ���� �� ��� ��������� ��'����, �� �������� � ����� \"���������� ����������\".");
                        return;
                    }
                }

                // ��������� � ������� �������
                string insertProperty = @"INSERT INTO property 
                (description, owner, phone_number, operation_type, property_type, district, number_of_rooms, price, floor, living_area) 
                VALUES (@desc, @owner, @phone, @optype, @ptype, @district, @rooms, @price, @floor, @living)";

                using (MySqlCommand cmd = new MySqlCommand(insertProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", addDescription.Text);
                    cmd.Parameters.AddWithValue("@owner", addOwner.Text);
                    cmd.Parameters.AddWithValue("@phone", addNumber.Text);
                    cmd.Parameters.AddWithValue("@optype", addRent.Checked ? "������" : "������");
                    cmd.Parameters.AddWithValue("@ptype", addProperty.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@district", addDistrict.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(addRoom.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(addPrice.Text));
                    cmd.Parameters.AddWithValue("@floor", int.Parse(addFloor.Text));
                    cmd.Parameters.AddWithValue("@living", decimal.Parse(addLivingArea.Text));
                    cmd.ExecuteNonQuery();
                }

                // ��������� ID ����� ������
                object result = new MySqlCommand("SELECT LAST_INSERT_ID()", conn).ExecuteScalar();
                long newID = Convert.ToInt64(result);

                // ��������� ����� ������� ��� ������� � ������� house_details
                if (addProperty.SelectedItem.ToString() == "�������")
                {
                    string insertHouse = "INSERT INTO house_details (id_property, land_area) VALUES (@id, @land)";
                    using (MySqlCommand cmd = new MySqlCommand(insertHouse, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", newID);
                        cmd.Parameters.AddWithValue("@land", decimal.Parse(addLandArea.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                // ��������� ������� � ������ ��� ������ � rental_details
                if (addRent.Checked)
                {
                    string insertRent = "INSERT INTO rental_details (id_property, pets_allow, apartment_status) VALUES (@id, @pets, '�������')";
                    using (MySqlCommand cmd = new MySqlCommand(insertRent, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", newID);
                        cmd.Parameters.AddWithValue("@pets", addPetsAllowed.Checked ? 1 : 0);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("��'��� ������ ������ �� ���� �����!");
                addClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // ���� ��� ����� ��� �������� ����� ���������� � ����� �����������
        private void editChoseDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDesc = editChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc)) return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // ������ ����
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
                            editRent.Checked = operation == "������";
                            editSale.Checked = operation == "������";
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

                // ��������� ���� � ������� ���� ������ �������
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

                // ��������� ���� � ��������� ���� ��'��� ��� ������
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

        // ������ ��� ���������� ��� ��� ����� � ��� �����
        private void Update_Click(object sender, EventArgs e)
        {
            if (!areEditFieldsFilled())
            {
                MessageBox.Show("���� �����, �������� �� ����!");
                return;
            }

            if (!AreAllInt(addFloor))
                return;

            if (!AreAllDecimals(addPrice, addLivingArea, addLandArea.Visible ? addLandArea : null))
                return;

            if (!validPhoneNumber(editNumber.Text))
            {
                MessageBox.Show("�������� ����������� ������� ������ ��������. \n�������� ������: +��� (��� +��) �� ��� ����");
                return;
            }

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // �������� ���������� �����
                string checkQuery = "SELECT COUNT(*) FROM property WHERE description = @desc AND id_property != @id";
                using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", editDescription.Text);
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("��'��� �� ����� ������ ��� ����!");
                        return;
                    }
                }

                // ��������� ������� �������
                string updateProperty = @"UPDATE property SET description = @desc, owner = @owner, phone_number = @phone, operation_type = @optype, 
                property_type = @ptype, district = @district, number_of_rooms = @rooms, price = @price, floor = @floor, living_area = @living
                WHERE id_property = @id";

                using (MySqlCommand cmd = new MySqlCommand(updateProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@id", editID.Text);
                    cmd.Parameters.AddWithValue("@desc", editDescription.Text);
                    cmd.Parameters.AddWithValue("@owner", editOwner.Text);
                    cmd.Parameters.AddWithValue("@phone", editNumber.Text);
                    cmd.Parameters.AddWithValue("@optype", editRent.Checked ? "������" : "������");
                    cmd.Parameters.AddWithValue("@ptype", editProperty.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@district", editDistrict.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(editRoom.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(editPrice.Text));
                    cmd.Parameters.AddWithValue("@floor", int.Parse(editFloor.Text));
                    cmd.Parameters.AddWithValue("@living", decimal.Parse(editLivingArea.Text));
                    cmd.ExecuteNonQuery();

                    // ��������� ����� � house_details ���� ��� ��� �� �������
                    if (editProperty.SelectedItem.ToString() != "�������")
                    {
                        string deleteHouse = "DELETE FROM house_details WHERE id_property = @id";
                        using (MySqlCommand cmdDel = new MySqlCommand(deleteHouse, conn))
                        {
                            cmdDel.Parameters.AddWithValue("@id", editID.Text);
                            cmdDel.ExecuteNonQuery();
                        }
                    }

                    // ��������� ����� � rental_details ���� ��'��� ��� �� ��� ������
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

                // ��������� ��� ����� ������ ���� ������ �������
                if (editProperty.SelectedItem.ToString() == "�������")
                {
                    string houseQuery = "REPLACE INTO house_details (id_property, land_area) VALUES (@id, @land)";
                    using (MySqlCommand cmd = new MySqlCommand(houseQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", editID.Text);
                        cmd.Parameters.AddWithValue("@land", decimal.Parse(editLandArea.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                // ��������� ��� �������� �����
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

                MessageBox.Show("���������� ������ ��������!");
                editClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // ���� ��� ����� ��� �������� ����� ���������� � ����� ���������
        private void deleteChoseDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDesc = deleteChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc)) return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // ������ ����
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
                            deleteRent.Checked = operation == "������";
                            deleteSale.Checked = operation == "������";

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

                // �������� ��� ��� ������
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

                // �������� ��� ��� �������
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

        // ������ ��� ��������� ��'����
        private void Delete_Click(object sender, EventArgs e)
        {
            string selectedDesc = deleteChoseDescription.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDesc))
            {
                MessageBox.Show("������ ��'��� ��� ��������� � �������� ������!");
                return;
            }
            
            // ������������ ���������
            var confirmResult = MessageBox.Show($"�� �������, �� ������ �������� ��'���: \"{selectedDesc}\"?", "ϳ��������� ���������", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult != DialogResult.Yes)
                return;

            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {
                conn.Open();

                // ������ id �� ������
                int idToDelete = 0;
                string getIdQuery = "SELECT id_property FROM property WHERE description = @desc";
                using (MySqlCommand cmd = new MySqlCommand(getIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", selectedDesc);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("��'��� �� ��������! ������ �������� ���� � �������� ������!");
                        return;
                    }
                    idToDelete = Convert.ToInt32(result);
                }

                // ��������� ����� � rental_details
                string deleteRent = "DELETE FROM rental_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteRent, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                // ��������� ����� � house_details
                string deleteHouse = "DELETE FROM house_details WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteHouse, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                // ��������� ����� � property
                string deleteProperty = "DELETE FROM property WHERE id_property = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteProperty, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idToDelete);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("��'��� ������ ��������!");
                deleteClean.PerformClick();
                LoadDescriptions();
                LoadGridData();
            }
        }

        // ���� ��� ������ � dataGridView2 �� ������� "��������� ��'������"
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
            dataGridView2.Columns["description"].HeaderText = "����";
            dataGridView2.Columns["owner"].HeaderText = "�������";
            dataGridView2.Columns["phone_number"].HeaderText = "�������";
            dataGridView2.Columns["operation_type"].HeaderText = "��������";
            dataGridView2.Columns["property_type"].HeaderText = "���";
            dataGridView2.Columns["district"].HeaderText = "�����";
            dataGridView2.Columns["number_of_rooms"].HeaderText = "ʳ�����";
            dataGridView2.Columns["price"].HeaderText = "ֳ��";
            dataGridView2.Columns["floor"].HeaderText = "������";
            dataGridView2.Columns["living_area"].HeaderText = "������� �����";
            dataGridView2.Columns["pets_allow"].HeaderText = "��������� ������";
            dataGridView2.Columns["apartment_status"].HeaderText = "������ ��������";
            dataGridView2.Columns["land_area"].HeaderText = "����� ������";
        }

        // �������� ����������� �������� � ����� "��" �� "��"
        private bool ValidateRange(TextBox fromBox, TextBox toBox, string fieldName)
        {
            int from = 0, to = 0;

            // �������� �� �� �����
            if (!string.IsNullOrWhiteSpace(fromBox.Text) && !int.TryParse(fromBox.Text, out from))
            {
                MessageBox.Show($"� ����� '{fieldName}' �������� '��' �� ���� ��������.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(toBox.Text) && !int.TryParse(toBox.Text, out to))
            {
                MessageBox.Show($"� ����� '{fieldName}' �������� '��' �� ���� ��������.");
                return false;
            }

            // �������� ��������
            if (!string.IsNullOrWhiteSpace(fromBox.Text) && !string.IsNullOrWhiteSpace(toBox.Text))
            {
                if (from > to)
                {
                    MessageBox.Show($"� ����� '{fieldName}' �������� '��' ������� ���� ������ �� '��'.");
                    return false;
                }
            }

            return true;
        }

        // ������ ������ ������ � ��������� ������ � ���� ����� �� �������� ���������
        private void Search_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = DBUtils.GetDBConnection())
            {

                if (!ValidateRange(searchPriceFrom, searchPriceTo, "ֳ��") || 
                    !ValidateRange(searchFloorFrom, searchFloorTo, "������") ||
                    !ValidateRange(searchLivingAreaFrom, searchLivingAreaTo, "������� �����") || 
                    !ValidateRange(searchLandAreaFrom, searchLandAreaTo, "����� ������"))
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

                // ���� ��������
                if (searchRent.Checked)
                {
                    conditions.Add("p.operation_type = '������'");
                    conditions.Add("r.apartment_status = '�������'");
                    if (searchPetsAllowed.Checked)
                        conditions.Add("r.pets_allow = 1");
                    else
                        conditions.Add("r.pets_allow = 0");
                }
                else if (searchSale.Checked)
                {
                    conditions.Add("p.operation_type = '������'");
                }

                // ��� ����������
                if (searchProperty.SelectedIndex != -1)
                {
                    conditions.Add("p.property_type = @ptype");
                    cmd.Parameters.AddWithValue("@ptype", searchProperty.SelectedItem.ToString());
                }

                // �����
                if (searchDistrict.SelectedIndex != -1)
                {
                    conditions.Add("p.district = @district");
                    cmd.Parameters.AddWithValue("@district", searchDistrict.SelectedItem.ToString());
                }

                // ������� �����
                if (searchRoom.SelectedIndex != -1)
                {
                    conditions.Add("p.number_of_rooms = @rooms");
                    cmd.Parameters.AddWithValue("@rooms", int.Parse(searchRoom.SelectedItem.ToString()));
                }

                // � ���������
                if (searchPetsAllowed.Checked)
                {
                    conditions.Add("r.pets_allow = 1");
                }

                // ������� � �����, ��������, �������� ������ � �������
                AddRangeFilter("p.price", searchPriceFrom, searchPriceTo, cmd, conditions);
                AddRangeFilter("p.floor", searchFloorFrom, searchFloorTo, cmd, conditions);
                AddRangeFilter("p.living_area", searchLivingAreaFrom, searchLivingAreaTo, cmd, conditions);
                AddRangeFilter("h.land_area", searchLandAreaFrom, searchLandAreaTo, cmd, conditions);

                // �������� ������� �������� ����
                if (conditions.Count > 0)
                    baseQuery += " WHERE " + string.Join(" AND ", conditions);

                cmd.CommandText = baseQuery;
                cmd.Connection = conn;

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    dataGridView1.Columns["description"].HeaderText = "����";
                    dataGridView1.Columns["owner"].HeaderText = "�������";
                    dataGridView1.Columns["phone_number"].HeaderText = "�������";
                    dataGridView1.Columns["operation_type"].HeaderText = "��������";
                    dataGridView1.Columns["property_type"].HeaderText = "���";
                    dataGridView1.Columns["district"].HeaderText = "�����";
                    dataGridView1.Columns["number_of_rooms"].HeaderText = "ʳ�����";
                    dataGridView1.Columns["price"].HeaderText = "ֳ��";
                    dataGridView1.Columns["floor"].HeaderText = "������";
                    dataGridView1.Columns["living_area"].HeaderText = "������� �����";
                    dataGridView1.Columns["pets_allow"].HeaderText = "��������� ������";
                    dataGridView1.Columns["apartment_status"].HeaderText = "������ ��������";
                    dataGridView1.Columns["land_area"].HeaderText = "����� ������";

                }
            }
        }

        // ��������� ���� ��� �������� ������� � WHERE �����
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
