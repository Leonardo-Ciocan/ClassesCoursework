using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace TimetableApp
{
    /// <summary>
    /// This class deals with validating strings. It also contains the validation constants.
    /// </summary>
    public class Validator
    {

        #region Validation Constants
        //Those constants will be used for validating input
        public static int TimetableTitleMax = 30;
        public static int TimetableDescriptionMax = 200;

        public const int EventClassMax = 7;
        public const int EventNotesMax = 300;
        public const int EventTitleMax = 20;
        public const int EventTeacherMax = 25;

        public const int HomeworkDescriptionMax = 500;

        public const int ModuleNameMax = 20;

        public const int DefinitionNameMax = 35;
        public const int DefinitionDescriptionMax = 300;

        public const int StructureBlockNameMax = 30;
        public const int TopicTitleMax = 20;

        #endregion

        /// <summary>
        /// This method is used to check whether a string meets certain validation rules. It can also autofix it and report it to the user.
        /// </summary>
        /// <param name="target">The string to check</param>
        /// <param name="scope">The context that describes the target (i.e Name)</param>
        /// <param name="allowDigits">Whether the string is allowed to contain numbers</param>
        /// <param name="allowEmpty">Whether the string can be empty</param>
        /// <param name="max">The maximum number of characteers.Including whitespace.</param>
        /// <param name="min">The minimum number of characteers.Including whitespace.</param>
        /// <param name="autoFix">If true , automatically fix the string (remove digits , remove character over the max limit)</param>
        /// <param name="fixTarget">The holder of the target.</param>
        /// <param name="autoNotify">Whether the method should display the errors found to the user. See ValidatorResponse.NotifyUser()</param>
        /// <param name="fileName">Whether the method should check if the string is a valid file path </param>
        /// <returns></returns>
        public static ValidatorResponse isStringValid(
            string target,string scope, bool allowDigits , bool allowEmpty, int max = int.MaxValue,
            int min = int.MinValue , bool autoFix = false , TextBox fixTarget=null ,bool autoNotify = false , bool fileName = false)
        {
            var errors = new List<string>();
            if(target.Length > max) errors.Add("The "+scope+" should be less than " + max + " characters.");
            if (target.Length < min) errors.Add("The " + scope + " should be more than " + max+ " characters.");
            if (!allowDigits) if (target.ToCharArray().Any(char.IsDigit)) errors.Add("The " + scope + " should not contain any numbers");
            if (!allowEmpty) if (target == "") errors.Add("The " + scope + " can't be empty");
            if (fileName)
            {
                errors.AddRange(from char c in target 
                                from k in System.IO.Path.GetInvalidFileNameChars() 
                                where c == k select 
                                "Character " + c + " is invalid.");
            }

            var response = new ValidatorResponse { isValid = (errors.Count == 0), Errors = errors };

            if (autoNotify) response.NotifyUser();
            if (autoFix && fixTarget != null)
            {
                if (target.Length > max) fixTarget.Text = fixTarget.Text.Substring(0, max);
                if (!allowDigits) if (target.ToCharArray().Any(char.IsDigit))
                    {
                        fixTarget.Text = new string(target.ToCharArray().Where(x => !char.IsDigit(x)).ToArray());
                    }
               // if (!allowEmpty) fixTarget.Text = "Enter a "+scope;
            }
            return response;
        }


        
    }   

    
    /// <summary>
    /// This is the object returned by the Validator method.
    /// </summary>
    public struct ValidatorResponse
    {
        public bool isValid;
        public List<string> Errors;

        /// <summary>
        /// Groups and outputs the errors of this response to the user.
        /// </summary>
        public async void NotifyUser()
        {
            if (Errors.Count > 0)
            {
                var errorMessage = Errors.Aggregate("The following errors have been fixed for you:\n", (current, s) => current + (s + "\n"));

                var dialog = 
                    new MessageDialog(content: errorMessage, title: "An error occured");
                await dialog.ShowAsync();
            }
        }
    }
}
