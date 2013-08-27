using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp
{

        //Represents a homework
        public class HomeworkTask
        {
            public DateTime DueOn = DateTime.Now.AddDays(7);//the time when the homework is due
            public string Notes = ""; //description of homework
            public bool Completed = false;//whether it's completed

           
        }

        //represents a definition
        public class Definition
        {
            public string Caption = "";//The term defined
            public string Content = "";//Its definition
        }

        //A module which contains its own set of notes/definitions etc
        public class Module
        {
            public string Name = "";
            //Lists of items
            public List<StructureBlock> StructureBlocks = new List<StructureBlock>();
            public List<Definition> Definitions = new List<Definition>();
            public List<HomeworkTask> Homeworks = new List<HomeworkTask>();
            public List<Note> Notes = new List<Note>();
            
        }

        //A structure block is the name of a topic group but it's abstracted from the user by just calling it topic
        public class StructureBlock
        {
            public string Name = "";
            public List<Topic> Topics = new List<Topic>();
        }

        //Represents a single topic
        public class Topic
        {
            public bool Studied = false;
            public bool Revised = false;
            public string Name = "Enter a topic name";
        }

        //Represents a single Note
        public class Note
        {
            public string Name = "Name this note";
            public string Description = "Add a description";
            public DateTime TimeCreated = DateTime.Now;
            public List<NoteFragment> Fragments = new List<NoteFragment>();
        }

        //A paragraph in a note. Has its own class for later extensibility.
        public class NoteFragment
        {
            public string Content = "Turn on writing mode on the left to start editing this.";
        }


}

    

   

