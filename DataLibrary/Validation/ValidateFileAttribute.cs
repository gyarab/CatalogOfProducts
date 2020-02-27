using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLibrary.Validation
{
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = false;
            var file = value as HttpPostedFileBase;

            if (file == null || file.ContentLength > 5 * 1024 * 1024)
            {
                return isValid;
            }

            if (IsFileTypeValid(file))
            {
                isValid = true;
            }

            return isValid;
        }

        private bool IsFileTypeValid(HttpPostedFileBase file)
        {
            bool isValid = false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (IsOneOfValidFormats(img.RawFormat))
                    {
                        isValid = true;
                    }
                }
            }
            catch
            {
                
            }
            return isValid;
        }

        private bool IsOneOfValidFormats(ImageFormat rawFormat)
        {
            List<ImageFormat> formats = getValidFormats();

            foreach (ImageFormat format in formats)
            {
                if (rawFormat.Equals(format))
                {
                    return true;
                }
            }
            return false;
        }

        private List<ImageFormat> getValidFormats()
        {
            List<ImageFormat> formats = new List<ImageFormat>
            {
                ImageFormat.Png,
                ImageFormat.Jpeg,
                ImageFormat.Gif
            };

            return formats;
        }
    }
}

