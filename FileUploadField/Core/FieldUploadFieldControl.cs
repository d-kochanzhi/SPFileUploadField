using FileUploadField.Core;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileUploadField
{
    internal class FileUploadFieldControl : Microsoft.SharePoint.WebControls.BaseFieldControl
    {
        protected FileUpload UploadFileControl;
        protected Button UploadButton;
        protected Button DeleteButton;
        protected Label StatusLabel;
        protected HiddenField hdnFileName;
        protected Panel pnlUpload;



        public override void Focus()
        {
            if (Field == null || this.ControlMode == SPControlMode.Display)
            { return; }
            EnsureChildControls();
            UploadFileControl.Focus();
        }

        /// <summary>
        /// Значение текущего поля
        /// </summary>
        public override object Value
        {
            get
            {
                EnsureChildControls();
                if (hdnFileName.Value != string.Empty)
                    return hdnFileName.Value;
                else if (UploadFileControl.PostedFile != null)
                {
                    string strFileName = UploadFileControl.PostedFile.FileName.Substring(UploadFileControl.PostedFile.FileName.LastIndexOf("\\") + 1);
                    return strFileName;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                EnsureChildControls();
                hdnFileName.Value = (string)this.ItemFieldValue;
                StatusLabel.Text = "Файл: <a href='" + (string)this.ItemFieldValue + "' target='_blank'>Просмотр (" + Path.GetFileName((string)this.ItemFieldValue) + ")</a>";
                DeleteButton.Visible = true;
            }
        }

        public override void Validate()
        {
            if (ControlMode == SPControlMode.Display || !IsValid)
            {
                return;
            }

            base.Validate();

            if (Field.Required &&
                (Value == null || Value.ToString().Length == 0))
            {
                this.ErrorMessage = "Необходимо задать значение для этого обязательного поля";
                IsValid = false;
                return;
            }
        }

        protected override void RenderFieldForDisplay(HtmlTextWriter output)
        {
            HyperLink link = new HyperLink();

            // Display currently selected values
            string value = Helper.NullToStr(ItemFieldValue);
            if (!String.IsNullOrEmpty(value))
            {
                link.Text = value;
                link.NavigateUrl = SPContext.Current.Web.Site.Url + value;
                link.RenderControl(output);
            }
        }

        protected override void CreateChildControls()
        {
            if (Field == null || this.ControlMode == SPControlMode.Display)
            { return; }

            base.CreateChildControls();
            UploadFileControl = (FileUpload)TemplateContainer.FindControl("UploadFileControl");
            UploadButton = (Button)TemplateContainer.FindControl("UploadButton");
            DeleteButton = (Button)TemplateContainer.FindControl("DeleteButton");
            StatusLabel = (Label)TemplateContainer.FindControl("StatusLabel");
            hdnFileName = (HiddenField)TemplateContainer.FindControl("hdnFileName");
            pnlUpload = (Panel)TemplateContainer.FindControl("pnlUpload");
            UploadButton.Click += new EventHandler(UploadButton_Click);
            DeleteButton.Click += new EventHandler(DeleteButton_Click);
            if (hdnFileName.Value == string.Empty)
            {
                StatusLabel.Text = "Нет файла.";
                DeleteButton.Visible = false;
            }
            Controls.Add(StatusLabel);

            FileUploadField _field = (FileUploadField)this.Field;
            if (_field.UseIDasFolder && this.ControlMode == SPControlMode.New)
            {
                pnlUpload.Visible = false;
                StatusLabel.Text = "Для загрузки файла сохраните элемент.";
            }

            if (string.IsNullOrEmpty(_field.UploadDocumentLibrary))
            {
                pnlUpload.Visible = false;
                StatusLabel.Text = "Перейдите в настройки списка и указите параметр <UploadDocumentLibrary>.";
            }

            /*
               Стилизация контролов
               первый класс по умолчанию, следующий класс экземпляра (для кастомизации)
               */
            pnlUpload.CssClass = string.Join(" ", "UploadContainer", String.Concat(this.Field.TypeAsString, "_", this.Field.InternalName));
            StatusLabel.CssClass = string.Join(" ", "StatusContainer", String.Concat(this.Field.TypeAsString, "_", this.Field.InternalName));
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (UploadFileControl.PostedFile == null) return;

            FileUploadField _field = (FileUploadField)this.Field;
            SPSite sourceSite = SPControl.GetContextSite(Context);
            SPWeb sourceWeb = SPControl.GetContextWeb(Context);

            try
            {
                if (_field.UseElevatedPrivileges)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(sourceSite.ID))
                        {
                            using (SPWeb web = site.OpenWeb(sourceWeb.ID))
                            {
                                web.AllowUnsafeUpdates = true;
                                UploadFileJob(web);
                            }
                        }
                    });
                }
                else
                {
                    using (SPWeb web = sourceSite.OpenWeb(sourceWeb.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        UploadFileJob(web);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Загрузка файла :: Ошибка " + ex.Message;
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "Файл удален :: Успешно";
            hdnFileName.Value = string.Empty;
            DeleteButton.Visible = false;
        }

        private void UploadFileJob(SPWeb web)
        {
            FileUploadField _field = (FileUploadField)this.Field;
            SPList objList = web.Lists.GetList(Guid.Parse(_field.UploadDocumentLibrary), false);

            SPFolder destFolder = objList.RootFolder;
            if (_field.UseIDasFolder) destFolder = destFolder.SubFolders.Add(this.ListItem.ID.ToString());

            string strFileName = UploadFileControl.PostedFile.FileName.Substring(UploadFileControl.PostedFile.FileName.LastIndexOf("\\") + 1);

            if (!string.IsNullOrEmpty(strFileName))
            {
                if (!string.IsNullOrEmpty(_field.RenameFile))
                {
                    strFileName = _field.RenameFile + "." + strFileName.Split('.')[strFileName.Split('.').Count() - 1];
                }

                Stream fStream = UploadFileControl.PostedFile.InputStream;
                SPFile objFile = destFolder.Files.Add(strFileName, fStream, true);
                objFile.Item.SystemUpdate();
                StatusLabel.Text = "Загрузка файла :: Успешно <a href='" + objFile.ServerRelativeUrl + "' target='_blank'>Просмотр (" + strFileName + ")</a>";
                hdnFileName.Value = objFile.ServerRelativeUrl;
                DeleteButton.Visible = true;
            }
            else
            {
                //ничего не выбрано, нужно обнулить поле
                StatusLabel.Text = "Файл удален :: Успешно";
                hdnFileName.Value = string.Empty;
                DeleteButton.Visible = false;
            }
        }

        protected override string DefaultTemplateName
        {
            get
            {
                return "FileUploadControlTemplate";
            }
        }
    }
}