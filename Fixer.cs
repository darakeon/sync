using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sync.Properties;

namespace Sync
{
    class Fixer
    {
        public Fixer(DataGridViewRow row)
        {
            var updated = row.Cells[Resources.Interface_Column_Updated].Value.ToString();
            var obsolete = row.Cells[Resources.Interface_Column_Obsolete].Value.ToString();

            var relativePath = row.Cells[Resources.Interface_Column_Path].Value.ToString();
            var file = row.Cells[Resources.Interface_Column_FileName].Value.ToString();

            origin = new FileToWrite(updated, relativePath, file);
            destiny = new FileToWrite(obsolete, relativePath, file);
        }

        private FileToWrite origin { get; set; }
        private FileToWrite destiny { get; set; }



        public Boolean Copy()
        {
            var confirm = confirmBox(Resources.Fixer_SureCopy, origin.Name, destiny.Folder);

            if (!confirm)
                return false;

            var copied = copy(origin, destiny);

            return copied;
        }

        public Boolean Update()
        {
            var confirm = confirmBox(Resources.Fixer_SureOverwrite, origin.Name, destiny.Folder, destiny.Date, origin.Date);

            if (!confirm)
                return false;

            var copied = copy(origin, destiny);

            return copied;
        }

        public Boolean Delete()
        {
            var confirm = confirmBox(Resources.Fixer_SureDelete, origin.Name, origin.Folder);

            if (!confirm)
                return false;

            var deleted = delete(origin);

            return deleted;
        }

        public Boolean Rollback()
        {
            var confirm = confirmBox(Resources.Fixer_SureOverwrite, origin.Name, origin.Folder, origin.Date, destiny.Date);

            if (!confirm)
                return false;

            var copied = copy(destiny, origin);

            return copied;
        }


        
        private static Boolean confirmBox(String format, params object[] args)
        {
            var message = String.Format(format, args);

            var response = MessageBox.Show(message, Resources.SyncName, MessageBoxButtons.YesNo);

            return response == DialogResult.Yes;
        }



        private static Boolean copy(FileToWrite origin, FileToWrite destiny)
        {
            if (File.Exists(origin.Path))
            {
                File.Copy(origin.Path, destiny.Path, true);
                return true;
            }

            if (Directory.Exists(origin.Path))
            {
                copyDirectory(origin.Path, destiny.Path);
                return true;
            }

            throw new ApplicationException(Resources.Fixer_WeirdItemType);
        }

        private static void copyDirectory(String origin, String destiny)
        {
            Directory.CreateDirectory(destiny);

            var files = Directory.GetFiles(origin)
                .Select(f => new { Origin = f, Destiny = f.Replace(origin, destiny) });

            foreach (var file in files)
            {
                File.Copy(file.Origin, file.Destiny);
            }

            var dirs = Directory.GetDirectories(origin)
                .Select(d => new { Origin = d, Destiny = d.Replace(origin, destiny) });

            foreach (var dir in dirs)
            {
                copyDirectory(dir.Origin, dir.Destiny);
            }
        }



        private static Boolean delete(FileToWrite wrong)
        {
            if (File.Exists(wrong.Path))
            {
                File.Delete(wrong.Path);
                return true;
            }

            if (Directory.Exists(wrong.Path))
            {
                Directory.Delete(wrong.Path, true);
                return true;
            }

            throw new ApplicationException(Resources.Fixer_WeirdItemType);
        }


        



    }
}
