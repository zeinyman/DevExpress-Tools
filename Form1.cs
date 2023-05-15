using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestComboBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("ItemPrice", typeof(decimal));

            dt.Rows.Add("ItemA", "A", 1, 14.5);
            dt.Rows.Add("ItemB", "B", 2, 0.75);
            dt.Rows.Add("ItemC", "C", 3, 1);



            List<MyItems> lst = new List<MyItems>();
            lst.Add(new MyItems { MyItemName = "x1", MyItemCode = "x1", MyItemId = 1, MyItemPrice = 18 });
            lst.Add(new MyItems { MyItemName = "x2", MyItemCode = "x2", MyItemId = 2, MyItemPrice = 181 });
            lst.Add(new MyItems { MyItemName = "x3", MyItemCode = "x3", MyItemId = 3, MyItemPrice = 19 });
            lst.Add(new MyItems { MyItemName = "x4", MyItemCode = "x4", MyItemId = 4, MyItemPrice = 8 });

            lookUpEdit2.Properties.DataSource = lst;
            lookUpEdit2.Properties.ValueMember = "MyItemId";
            lookUpEdit2.Properties.DisplayMember = "MyItemName";

            lookUpEdit1.Properties.DataSource = dt;
            lookUpEdit1.Properties.PopulateColumns();
            lookUpEdit1.Properties.DisplayMember = "ItemName";
            lookUpEdit1.Properties.ValueMember = "ItemId";

            lookUpEdit1.Properties.Columns["ItemId"].Visible = false;
            lookUpEdit1.Properties.Columns["ItemName"].Caption = @"اسم الصنف";

            lookUpEdit1.Properties.NullText = @"الرجاء اختر الصنف أولا";


            lookUpEdit1.CustomDisplayText += LookUpEdit1_CustomDisplayText;

            lookUpEdit1.Properties.Columns["ItemPrice"].AllowSort = DevExpress.Utils.DefaultBoolean.False;
            lookUpEdit1.Properties.Columns["ItemPrice"].Alignment = DevExpress.Utils.HorzAlignment.Near;
            lookUpEdit1.ButtonClick += LookUpEdit1_ButtonClick;
            var btnclear = new DevExpress.XtraEditors.Controls.EditorButton
            {
                Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Clear,
                IsLeft = false,
                Tag = "Clear",
                Shortcut = new DevExpress.Utils.KeyShortcut(Keys.Control | Keys.Delete)

            };
            var imgbtn = new DevExpress.XtraEditors.Controls.EditorButton
            {
                Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph,
                IsLeft = false,
                Tag = "Add",
                Shortcut = new DevExpress.Utils.KeyShortcut(Keys.Shift | Keys.A)
                ,
                ToolTip = "To Add A new Item (CTRL+A)"

            };
            imgbtn.ImageOptions.ImageUri = @"Add;Size16x16";
            lookUpEdit1.Properties.Buttons.AddRange(new[] { imgbtn, btnclear });


            searchLookUpEdit1.Properties.DataSource = dt;
            searchLookUpEdit1.Properties.DisplayMember = "ItemName";
            searchLookUpEdit1.Properties.ValueMember = "ItemId";
            //searchLookUpEdit1.Properties.View.PopulateColumns();

            searchLookUpEdit1.ForceInitialize();
            searchLookUpEdit1.Properties.PopulateViewColumns();

            searchLookUpEdit1.Properties.View.Columns["ItemId"].Visible = false;
            searchLookUpEdit1.Properties.View.Columns["ItemName"].Caption = @"اسم الصنف";

            searchLookUpEdit1.EditValueChanged += SearchLookUpEdit1_EditValueChanged;


            //searchLookUpEdit1.CustomDisplayText += SearchLookUpEdit1_CustomDisplayText;
            //searchLookUpEdit1.QueryPopUp += searchLookUpEdit1_QueryPopUp;
        }

        private void SearchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEdit1.GetSelectedDataRow() == null) textEdit1.Text = "بدون";
            else
            {
                //var mtRow = searchLookUpEdit1.GetSelectedDataRow() as DataRowView;

                //textEdit1.Text = mtRow?["ItemCode"].ToString();

                if (searchLookUpEdit1.GetSelectedDataRow() is DataRowView row)
                {
                    //textEdit1.Text = row["ItemCode"].ToString();
                    textEdit1.EditValue = MyClass.GetColumnValue(searchLookUpEdit1, "ItemCode");
                }


            }
        }

        private void searchLookUpEdit1_QueryPopUp(object sender, CancelEventArgs e)
        {

            searchLookUpEdit1.Properties.View.Columns["ItemId"].Visible = false;
            searchLookUpEdit1.Properties.View.Columns["ItemName"].Caption = @"اسم الصنف";


        }
        private void SearchLookUpEdit1_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            //if (searchLookUpEdit1.GetSelectedDataRow() == null) e.DisplayText = null;
            //else
            //{
            //    e.DisplayText= 
            //}
        }

        private void LookUpEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!e.Button.Enabled || e.Button.Tag == null) return;


            if (string.Equals(e.Button.Tag.ToString(), "clear", StringComparison.InvariantCultureIgnoreCase))
            {
                lookUpEdit1.EditValue = null;
            }
            else if (string.Equals(e.Button.Tag.ToString(), "Add", StringComparison.InvariantCultureIgnoreCase))
            {
                DataTable dt = (DataTable)lookUpEdit1.Properties.DataSource;
                dt.Rows.Add("NewOne" + dt.Rows.Count, "Code" + dt.Rows.Count, dt.Rows.Count, dt.Rows.Count * 1.2);

            }
        }

        private void LookUpEdit1_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (lookUpEdit1.GetSelectedDataRow() == null) e.DisplayText = lookUpEdit1.Properties.NullText;
            else
            {
                var ItemName = MyClass.GetColumnValue(lookUpEdit1, "ItemName").ToString();
                var ItemPrice = Convert.ToDecimal(MyClass.GetColumnValue(lookUpEdit1, "ItemPrice"));
                e.DisplayText = $@"{ItemName} ({ItemPrice:0.00})";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            MessageBox.Show(MyClass.GetColumnValue(lookUpEdit1)?.ToString());
            MessageBox.Show(MyClass.GetColumnValue(lookUpEdit1, "ItemCode")?.ToString());

            var choice = MyClass.GetColumnValue<MyItems>(lookUpEdit2) as MyItems;
            if (choice != null)
            {
                MessageBox.Show(choice.MyItemCode);
                MessageBox.Show(choice.MyItemPrice.ToString());
                MessageBox.Show(choice.ToString());
            }

            //if (lookUpEdit1.GetSelectedDataRow() != null)
            //{
            //    if (lookUpEdit1.GetSelectedDataRow() is DataRowView myrow)
            //    {
            //        MessageBox.Show($@"Code={myrow["ItemCode"]}, Price={myrow["ItemPrice"]}");
            //    }
            //}
            //if (lookUpEdit2.GetSelectedDataRow() is MyItems item)
            //{
            //    MessageBox.Show($@"Code={item.MyItemCode}, Price={item.MyItemPrice}");

            //}


            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {



            //if (lookUpEdit1.GetSelectedDataRow() == null)
            //{
            //    MessageBox.Show(lookUpEdit1.Properties.NullText);
            //    lookUpEdit1.ShowPopup();
            //    return;
            //}

            lookUpEdit1.EditValue = null;
            //lookUpEdit2.EditValue = 2;
        }


    }

    public class MyItems
    {
        public string MyItemName { get; set; }
        public string MyItemCode { get; set; }
        public int MyItemId { get; set; }
        public decimal MyItemPrice { get; set; }

        public override string ToString()
        {
            return $@"ItemName={MyItemName},Code={MyItemCode},Price={MyItemPrice:0.##} ";
        }
    }
    internal class MyClass
    {
        //public static object GetColumnValue(LookUpEdit look, string columnName = null)
        //{
        //    if (look.GetSelectedDataRow() == null) return null;

        //    //if (string.IsNullOrEmpty(columnName) || string.IsNullOrWhiteSpace(columnName)) return null;
        //    //if (look.Properties.Columns?.Count == 0) return null;
        //    //string valuemember = columnName == null ? look.Properties.ValueMember : columnName;
        //    string valuemember = columnName ?? look.Properties.ValueMember;

        //    if (look.Properties.Columns is { Count: > 0 } && look.Properties.Columns[valuemember] != null)
        //    {
        //        return ((DataRowView)look.GetSelectedDataRow())[valuemember];
        //    }
        //    return null;

        //}
        //public static object GetColumnValue(SearchLookUpEdit look, string columnName = null)
        //{
        //    if (look.GetSelectedDataRow() == null) return null;

        //    //if (string.IsNullOrEmpty(columnName) || string.IsNullOrWhiteSpace(columnName)) return null;
        //    //if (look.Properties.Columns?.Count == 0) return null;
        //    //string valuemember = columnName == null ? look.Properties.ValueMember : columnName;
        //    string valuemember = columnName ?? look.Properties.ValueMember;

        //    if (look.Properties.View.Columns is { Count: > 0 } && look.Properties.View.Columns[valuemember] != null)
        //    {
        //        return ((DataRowView)look.GetSelectedDataRow())[valuemember];
        //    }
        //    return null;

        //}
        public static object GetColumnValue<MyType>(LookUpEdit look)
        {
            if (look.GetSelectedDataRow() == null) return null;
            if (look.GetSelectedDataRow() is MyType) return (MyType)look.GetSelectedDataRow();
            return null;

        }
        public static object GetColumnValue<MyType>(SearchLookUpEdit look)
        {
            if (look.GetSelectedDataRow() == null) return null;
            if (look.GetSelectedDataRow() is MyType) return (MyType)look.GetSelectedDataRow();
            return null;

        }
        public static object GetColumnValue( LookUpEditBase look, string columnName = null)
        {
            if (look.GetSelectedDataRow() == null) return null;
            if (look is SearchLookUpEdit slook)
            {
                string valuemember = columnName ?? slook.Properties.ValueMember;

                if (slook.Properties.View.Columns is { Count: > 0 } && slook.Properties.View.Columns[valuemember] != null)
                {
                    return ((DataRowView)slook.GetSelectedDataRow())[valuemember];
                }
               
            }
            else if (look is LookUpEdit mylook)
            {
                string valuemember = columnName ?? mylook.Properties.ValueMember;

                if (mylook.Properties.Columns is { Count: > 0 } && mylook.Properties.Columns[valuemember] != null)
                {
                    return ((DataRowView)mylook.GetSelectedDataRow())[valuemember];
                }
            }

            return null;


        }
    }
}
