using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace TimetableApp
{
    public static class CoreData
    {
        //Local folder contains the folder called Data that holds our files
        static StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
        static StorageFolder DataFolder;

        //List of timetables that is accessible from anywhere in the app
        public static List<StudentDataBundle> Timetables = new List<StudentDataBundle>();

        //Reusable serializer for writing and reading .tb files
        public static XmlSerializer Serializer = new XmlSerializer(typeof(StudentDataBundle));



        /// <summary>
        /// Used to locate the data folder for further operations - must be called any entry point of the app.
        /// </summary>
        /// <returns>Whether an error occured.</returns>
        public static async Task<bool> Initialize()
        {
            try
            {
                //Try to create the data folder  , or open it if it exists
                DataFolder = await LocalFolder.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This will create a timetable file from a studentDataBundle instance
        /// </summary>
        /// <param name="studentDataBundle">The timetable to save</param>
        /// <returns></returns>
        public static async Task<bool> CreateTimetable(StudentDataBundle studentDataBundle)
        {
            var found = false; //whether a timetable with the same name exists
            try
            {
                //try to get timetable file with same name
                await DataFolder.GetFileAsync(studentDataBundle.Name + ".tb");
            }
            catch 
            {
                found = true;
            }

            //if it's found , then it cannot be made. Another name must be chosen
            if (!found)
            {
                var dialog = new MessageDialog("A StudentDataBundle with that name already exists.\nPlease chooose another name.", "Oops");
                await dialog.ShowAsync();
                return false;
            }
            else
            {
                //create the file and asynchronously serialize the data bundle to xml then add the object to the list
                StorageFile file = await DataFolder.CreateFileAsync(studentDataBundle.Name +".tb");
                Stream filestream = await file.OpenStreamForWriteAsync();
                await Task.Run(() => Serializer.Serialize(filestream, studentDataBundle));
                Timetables.Add(studentDataBundle);
                return true;//return success
            }
        }

        /// <summary>
        /// Edit name or description of  timetable
        /// </summary>
        /// <param name="table">The edit target</param>
        /// <param name="rename">Whether it should be renamed</param>
        /// <param name="newName">The new name (if rename = true)</param>
        /// <returns></returns>
        public static async Task<bool> EditTimetable(StudentDataBundle table,bool rename=false , string newName = "")
        {
            try
            {
                //get the file and overwrite with new data
                    var file = await DataFolder.GetFileAsync(table.Name + ".tb");
                    table.Name = newName;
                    Stream dataStream = await file.OpenStreamForWriteAsync();
                    await Task.Run(() => Serializer.Serialize(dataStream, table));

                //if it should rename , rename it asynchrnously
                if (rename)
                    await file.RenameAsync(newName + ".tb");
                return true;

            }
            catch
            {
                return false;//error
            }

        }

        /// <summary>
        /// Fills the list with all the timetables
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> GetTimetables()
        {
            
            Timetables.Clear();//empty it just in case it has data
            var success = true;//assume it works

            var names = await DataFolder.GetFilesAsync(); //list of all files in data folder
                foreach (var name in names)
                {
                    
                    Stream st = await name.OpenStreamForReadAsync();//open a read-only data stream
                   
                    //asynchrnously deserialize each file
                    await Task.Run(() =>
                    {
                        try
                        {
                            StudentDataBundle table = Serializer.Deserialize(st) as StudentDataBundle;

                            Timetables.Add(table);
                        }
                        catch
                        {
                            success = false;
                        }
                    });
                    
                    st.Dispose();
                }
            return success;
        }

        /// <summary>
        /// This will delete a timetable
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteTimetable(StudentDataBundle table)
        {
            //bool success = true;
            try
            {
                //finds the file with that name and deletes. Then removes the object from the list.
                StorageFile file = await DataFolder.GetFileAsync(table.Name + ".tb");
                await file.DeleteAsync();
                Timetables.Remove(table);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Save a timetable over the old version
        /// </summary>
        /// <param name="table">The timetable to save</param>
        /// <returns></returns>
        public static async Task<bool> SaveTimetable(StudentDataBundle table)
        {
            try
            {
                //find timetable file , open for writing and serialize it
                StorageFile file = await DataFolder.CreateFileAsync(table.Name + ".tb", CreationCollisionOption.ReplaceExisting);
                Stream dataStream = await file.OpenStreamForWriteAsync();
                await Task.Run(() =>
                {
                    Serializer.Serialize(dataStream, table);
                });
                dataStream.Dispose();//to avoid two streams exceptions
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Export a timetable to a .tb file
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static async Task<bool> ExportTimetable(StudentDataBundle table)
        {
            //file picker to let the use choose a file
            FileSavePicker saver = new FileSavePicker();
            saver.DefaultFileExtension = ".tb";
            saver.SuggestedFileName = table.Name;
            saver.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            saver.FileTypeChoices.Add("StudentDataBundle file" , new List<string>{".tb"} );//.tb is the file extension
            StorageFile file = await saver.PickSaveFileAsync();//le the use pick a file
            //if the use did not press Cancel , it should not be null , so write to it
            if (file != null)
            {
                Stream stream = await file.OpenStreamForWriteAsync();
                await Task.Run(() =>
                {
                    Serializer.Serialize(stream, table);
                });
            }
            return true;
        }

        /// <summary>
        /// Imports a .tb file
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> ImportTimetable()
        {
            try
            {
                //let the user pick file
                FileOpenPicker opener = new FileOpenPicker();
                opener.FileTypeFilter.Add(".tb");
                opener.CommitButtonText = "Open StudentDataBundle";
                opener.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                StorageFile file = await opener.PickSingleFileAsync();
                Stream str = await file.OpenStreamForReadAsync();
                bool doRename = false;
                //if the user changed the filename it could break the app so it must be renamed
                await Task.Run(() =>
                {
                    StudentDataBundle table=Serializer.Deserialize(str) as StudentDataBundle;
                    if (file.Name != table.Name)
                    {
                        doRename = true;
                    }
                    Timetables.Add(table);
                    str.Dispose();
                });
                //copy it to the data folder
                file = await file.CopyAsync(DataFolder, Timetables.Last().Name + ".tb",NameCollisionOption.FailIfExists);
                //rename appropiately
                if (doRename) await file.RenameAsync(Timetables.Last().Name + ".tb" , NameCollisionOption.ReplaceExisting);            
                return true;
            }
            catch(Exception x)
            {
                return false;
            }
        }
    }
}
