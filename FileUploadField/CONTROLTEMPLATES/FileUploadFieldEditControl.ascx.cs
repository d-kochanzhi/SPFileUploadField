using FileUploadField.Core;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileUploadField
{
    public partial class FileUploadFieldEditControl : UserControl, Microsoft.SharePoint.WebControls.IFieldEditor
    {
        protected DropDownList ddlDocLibs;
        protected CheckBox chkUseId;
        protected CheckBox chkElevatedPrivileges;
        protected TextBox txtRename;

        private FileUploadField _field = null;

        public bool DisplayAsNewSection
        {
            get { return true; }
        }

        public void InitializeWithField(SPField field)
        {
            this._field = field as FileUploadField;
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            FileUploadField myField = field as FileUploadField;
            myField.UploadDocumentLibrary = ddlDocLibs.SelectedValue;
            myField.UseIDasFolder = chkUseId.Checked;
            myField.UseElevatedPrivileges = chkElevatedPrivileges.Checked;
            myField.RenameFile = txtRename.Text;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            SPListCollection objLists = SPContext.Current.Web.Lists;
            foreach (SPList objList in objLists)
            {
                if (objList is SPDocumentLibrary)
                    ddlDocLibs.Items.Add(new ListItem(objList.Title, objList.ID.ToString()));
            }
            if (!IsPostBack && _field != null)
            {
                //setup fields

                if (!String.IsNullOrEmpty(_field.UploadDocumentLibrary))
                {
                    var el = Helper.FindControlRecursive<DropDownList>(this, "ddlDocLibs").Items.FindByValue(_field.UploadDocumentLibrary);
                    if (el != null)
                        el.Selected = true;
                }

                Helper.FindControlRecursive<CheckBox>(this, "chkUseId").Checked = _field.UseIDasFolder;
                Helper.FindControlRecursive<CheckBox>(this, "chkElevatedPrivileges").Checked = _field.UseElevatedPrivileges;
                Helper.FindControlRecursive<TextBox>(this, "txtRename").Text = _field.RenameFile;
            }
        }


    }
}