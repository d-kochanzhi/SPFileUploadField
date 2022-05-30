using FileUploadField.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace FileUploadField
{
    public class FileUploadField : SPFieldText
    {
        public FileUploadField(SPFieldCollection fields, string fieldName) : base(fields, fieldName)
        {
            Init();
        }

        public FileUploadField(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName)
        {
            Init();
        }

        #region "PROPERTIES"

        private string _UploadDocumentLibrary = string.Empty;

        public string UploadDocumentLibrary
        {
            get
            {
                return _UploadDocumentLibrary;
            }
            set
            {
                this.SetCustomProperty("UploadDocumentLibrary", value);
                _UploadDocumentLibrary = value;
            }
        }

        private bool _UseIDasFolder = false;

        public bool UseIDasFolder
        {
            get
            {
                return _UseIDasFolder;
            }
            set
            {
                this.SetCustomProperty("UseIDasFolder", value);
                _UseIDasFolder = value;
            }
        }

        private bool _UseElevatedPrivileges = false;

        public bool UseElevatedPrivileges
        {
            get
            {
                return _UseElevatedPrivileges;
            }
            set
            {
                this.SetCustomProperty("UseElevatedPrivileges", value);
                _UseElevatedPrivileges = value;
            }
        }

        private string _RenameFile = string.Empty;

        public string RenameFile
        {
            get
            {
                return _RenameFile;
            }
            set
            {
                this.SetCustomProperty("RenameFile", value);
                _RenameFile = value;
            }
        }

        #endregion "PROPERTIES"

        private void Init()
        {
            this._UploadDocumentLibrary = Helper.NullToStr(this.GetCustomProperty("UploadDocumentLibrary"));
            this._UseIDasFolder = Helper.NullToBool(this.GetCustomProperty("UseIDasFolder"));
            this._UseElevatedPrivileges = Helper.NullToBool(this.GetCustomProperty("UseElevatedPrivileges"));
            this._RenameFile = Helper.NullToStr(this.GetCustomProperty("RenameFile"));
        }

        public override void Update()
        {
            this.SetCustomProperty("UploadDocumentLibrary", this.UploadDocumentLibrary);
            this.SetCustomProperty("UseIDasFolder", this.UseIDasFolder);
            this.SetCustomProperty("UseElevatedPrivileges", this.UseElevatedPrivileges);
            this.SetCustomProperty("RenameFile", this.RenameFile);
            base.Update();
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new FileUploadFieldControl();
                fieldControl.FieldName = InternalName;
                return fieldControl;
            }
        }


    }
}