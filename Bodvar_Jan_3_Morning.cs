/*
 * Project Name: Bodvar_Vintage_Car_Rental
 * Assignment for C# Course at IBAT College
 * Deadline 7 January 2018
 * 
 * Author: Bodvar Jonsson
 * Email: bjblackbelt@gmail.com
 * Telephone: 0858204129
 * 
 * General Description:
 * 
 * This project is a comprehensive version of the 
 * assignment for the C# course at IBAT College, 
 * wintersemester 2017-2018, deadline 7 January 2018.
 * 
 * Like the name indicates the project is about a 
 * car rental for vintage cars.
 * 
 * **********************************************************
 * 
 * Use of a Modified Version of Banklibrary:
 * 
 * Please note that the project has a reference to a slightly 
 * modified version of the project Banklibrary and its affiliated
 * files. Therefore it is important to use the Banklibrary project
 * I have provided and establish the reference to it.
 * The modifications are minor, consisting in a heading with the text
 * 'Catalog of Vintage Cars to Rent' in large purple letters, and
 * changing the four labels from the original Banklibrary project to
 * 'Number of Car', 'Car Brand', 'Model' and 'Rental Cost per Day'
 * respectively.
 * 
 * The Form1.cs file which inherits from the BankUIForm contained in 
 * the aforementioned Banklibrary has some additions:
 * 
 * - An image of a bluegreen colored car placed beneath the heading.
 * If it needs to be imported into the PictureBox again, please note 
 * that the filename is 'small_bluegreen_car.png' and it is located 
 * in the samee folder as this file here, namely 'Bodvar_Vintage_Car_Rental'.
 * Please take care not to the import the original Pixabay, and much larger,
 * image designated 'wiesmann-gt-mf4-2932846_640.png' and located in 
 * the same folder.
 * 
 * - Buttons each with their respective handlers designed to carry out
 * specific tasks:
 * 
 * - 'Step Down': step up in the list of catalog entries
 * 
 * - 'Step Up': step down in the list of catalog entries
 * 
 * - 'Add Element to Current Location': add an element to the 
 *   catalog at the current locations.  Elements in that location
 *   and above have their index increased by one.
 *    
 * - 'Add Element to End of List': an element is appended to the end
 *   of the list.
 *   
 * - 'Go to Initial Element of List': loading and displaying of the 
 *   very first element (index 0) of the list.
 *   
 * - 'Enter Modification to Current Element': when pressed, loads the 
 *   value that is in the text box fields at that time to the current element of 
 *   the list.
 *   Please note that if 'Add Element to Current Location' or 'Add
 *   Element to End of List' have been pressed, all buttons except
 *   'Enter Modification to Current Element' become disabled.
 *   Pressing 'Enter Modification to Current Element' implements change
 *   of value (and when applicable) addition of new element and last 
 *   but not least enables the other buttons again, if either of the 
 *   two aforementioned buttons for adding a new element to the list
 *   have been pressed.
 *   
 * - 'Delete Current Element': removes the current element from the 
 *   list showing the successive element of the list or (in case of the 
 *   last element being deleted) the previous one.
 *   
 * - 'Load from Binary File': gives the user the opportunity to load
 *   the serialized contents from a binary file of type RecordSerializable
 *   into the List<T> named 'catalogCarEntry' consisting of elements of
 *   type 'RecordSerializable'.
 *   I have loaded binary files of type .SER containing RecordSerialzable data
 *   with success.
 * 
 * - 'Sae to Binary File': gives the user the opportunity to output and save
 *   the contents of the List<T> 'catalogCarEntry' consisting of RecordSerializable
 *   elements into a .SER file of the user's choice.
 *   
 * - 'Exit': enables exiting of the application.
 * 
 *********************************************************************************
 * Ready List with 4 elements contained in the program:
 * 
 * When the user starts the program the List<T> named 'catalogCarEntry' is initialized
 * with values for 4 cars.  This makes it possible for the user to play around
 * with the functionality of the buttons before he loads a .SER file or saves
 * a .SER file.
 * 
 *********************************************************************************
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using BankLibrary;
using System.Collections;

namespace Bodvar_Vintage_Car_Rental
{
    public partial class Form1 : BankUIForm // Please note like previously mentioned
                                // that this is a slightly modified BankUIForm.
    {

        // object for serializing RecordSerializables in binary format
        private BinaryFormatter serializer = new BinaryFormatter();  // serializer is the name that Brian uses
                                                                     // in his example sent to me on 24 December.
                                                                     // This name (variable or whatever one should call it) is called 
                                                                     // 'formatter' in Fig17_09 project.

        private FileStream outputstream; // Stream for writing to a file.  This corresponds to the 
                                         // 'output' in the Fig17_09 project.
                                         // In email from Brian Rogers sent on 24 December he simply calls
                                         // a filestrea with a similar purpos simply 'stream'.

        private FileStream inputstream;  // Stream for reading from a file/
                                         // This corresponds to 'input' form the Fig17_10 project.
                                         // In Email sent from Brian 24 December it is called 'stream' 
                                         // To avoid ambiguity I have designated the Filestream for 
                                         // reading data from a file 'inputstream'. 
                                         // This helps avoid possible confusion with the stream for 
                                         // outputting data, which I have in the filestream preceding this
                                         // one listed a 'outputstream'.



        // 'catalogCarEntry' is the name of the 'List' (or List<T> if you will, T being a generic description for datatype) 
        //  or class of elements of type 
        // 'RecordSerializable':

        List<RecordSerializable> catalogCarEntry = new List<RecordSerializable>();

        // parameterless constructor
        public Form1()
        {
            InitializeComponent();

            // Creation of a List with catalog entries which is to help 
            // in development and testing out the different functinalities
            // of the program:

            // Creation of 4 catalog entries.  Please note that each entry has 4 fields, 
            // namely 'number in catalog', 'car brand', 'model' and 'rental cost per day'.

            // var dummyRecord = new RecordSerializable(0, "", "", 0.00M);
            var record1 = new RecordSerializable(1, "Ford", "Fiesta", 68.37M);
            var record2 = new RecordSerializable(2, "Chevrolet", "Corvette", 232.54M);
            var record3 = new RecordSerializable(3, "Dodge", "Ranger", 170.33M);
            var record4 = new RecordSerializable(4, "Fiat", "Uno", 250.45M);

            // Write 4 record entries into the list 'catalogCarEntry':

            catalogCarEntry.Add(record1);
            catalogCarEntry.Add(record2);
            catalogCarEntry.Add(record3);
            catalogCarEntry.Add(record4);

            // Creation and initialization of a variable to measure the length 
            // of the list.  This will serve an important purpose in checking 
            // errors and catching exception, like for example when the button
            // 'Step Up' has been pressed too often:

            int catalogLength = catalogCarEntry.Capacity;
        }

        RecordSerializable currentRecord; // In this 'record' named 'currentRecord' of type 
                                          // 'RecordSerializable' records to be displayed, 
                                          //  manipulated or deleted are to be stored.

        public int counter = 0;    // A counter value used modified when stepping up and down.

        public int oldCounter = 0; // An extra counter value to store previous counter value should it
                                   // be needed later.

        // Flags used by modifying event handler for deciding how to manipulate indexes and changes of 
        // values in the carCatalogEntry list:

        public Boolean justModifyCurrentValue = true; // This will be the default, so to speak.
        public Boolean insertValueInCurrentLocation = false;
        public Boolean appendValueToEndOfList = false;

        // Method for handling loading of the form:
        private void Form1_Load(object sender, EventArgs e)
        {
            // See to it that by default the first element of the list (element with index 0) gets displayed
            // when the file is loaded.  
            // Should the list be empty there will be a corresponding error handling procedure.

            // Note that the counter has been declared 0 at the very beginning of the program, so I won't be doing it
            // again here.

            if (catalogCarEntry.Count() != 0) // Making sure the list is not empty.
            {

                currentRecord = catalogCarEntry[counter];
                // store RecordSerializable values in temporary string array
                var values = new string[] {
                    currentRecord.Account.ToString(),
                    currentRecord.FirstName.ToString(),
                    currentRecord.LastName.ToString(),
                    currentRecord.Balance.ToString()
                };

                // It can't be overemphasized how important it is to keep track of the current counter value
                // as well as the overalall length of the list (the number of elements in it)
                // in the entire program.

                // determine whether TextBox account field is empty
                if (!string.IsNullOrEmpty(values[(int)TextBoxIndices.Account])) // this seems also to check perhaps
                                                                                // if list has not element, provided
                                                                                // a conversion of an empty list of
                                                                                // RecordSeriaizable elements gets
                                                                                // transformed into a Null during 
                                                                                // the transformation to the string
                                                                                // array above.

                {
                    // store TextBox values in RecordSerializable and setting to text 
                    // box values, provided that everything is OK as regards for 
                    // format and account number:
                    try
                    {
                        // get account-number value from TextBox
                        int accountNumber = int.Parse(
                        values[(int)TextBoxIndices.Account]);

                        // determine whether accountNumber is valid
                        if (accountNumber > 0)
                        {
                            // copy string-array values to TextBox values
                            SetTextBoxValues(values);

                        }
                        else
                        {
                            // notify user if invalid account number
                            MessageBox.Show("Invalid Account Number", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show("Invalid Format", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                else
                {
                    MessageBox.Show("List is Empty!", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method for handling step down click:
        private void step_down_Click(object sender, EventArgs e)
        {
            if (catalogCarEntry.Count() != 0) // Making sure the list is not empty.
            {
                // lower counter by 1, when possible:
                if (counter <= 0)
                {
                    counter = 0;
                    MessageBox.Show("Lower Bound of Catalog Reached", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    counter = counter - 1;
                }

                currentRecord = catalogCarEntry[counter];
                // store RecordSerializable values in temporary string array
                var values = new string[] {
                    currentRecord.Account.ToString(),
                    currentRecord.FirstName.ToString(),
                    currentRecord.LastName.ToString(),
                    currentRecord.Balance.ToString()
                };

                // It can't be overemphasized how important it is to keep track of the current counter value
                // as well as the overalall length of the list (the number of elements in it)
                // in the entire program.

                // determine whether TextBox account field is empty
                if (!string.IsNullOrEmpty(values[(int)TextBoxIndices.Account])) // this seems also to check perhaps
                                                                                // if list has not element, provided
                                                                                // a conversion of an empty list of
                                                                                // RecordSeriaizable elements gets
                                                                                // transformed into a Null during 
                                                                                // the transformation to the string
                                                                                // array above.

                {
                    // Checking first format of account number is OK.  If not format exception.
                    // If format is OK, then check if number is greater that 0.  Eventually
                    // display contents of the RecordSerializable as strings in the 
                    // corresponding text boxes: 
                    
                    try
                    {
                        // get account-number value from TextBox
                        int accountNumber = int.Parse(
                        values[(int)TextBoxIndices.Account]);

                        // determine whether accountNumber is valid
                        if (accountNumber > 0)
                        {
                            // copy string-array values to TextBox values
                            SetTextBoxValues(values);

                        }
                        else
                        {
                            // notify user if invalid account number
                            MessageBox.Show("Invalid Account Number", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show("Invalid Format", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                else
                {
                    MessageBox.Show("List is Empty!", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method for handing step up click:
        private void step_up_Click(object sender, EventArgs e)
        {
            if (catalogCarEntry.Count() != 0) // Making sure the list is not empty.
            {
                // If list is not empty go up the list when possible:

                if ((counter + 1) == catalogCarEntry.Count()) // Check if counter is currently on the last (highest) element 
                {                                        // of the list.  Note that one needs to be added, because inexing
                                                         // starts at 0.

                    MessageBox.Show("Upper Bound of Catalog Reached", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // The counter is not incremented.  It remains in the value that it was in before entering the module.
                }
                else
                {
                    counter = counter + 1;
                }

                currentRecord = catalogCarEntry[counter];
                // store RecordSerializable values in temporary string array
                var values = new string[] {
                        currentRecord.Account.ToString(),
                        currentRecord.FirstName.ToString(),
                        currentRecord.LastName.ToString(),
                        currentRecord.Balance.ToString()
                    };

                // It can't be overemphasized how important it is to keep track of the current counter value
                // as well as the overalall length of the list (the number of elements in it)
                // in the entire program.

                // determine whether TextBox account field is empty
                if (!string.IsNullOrEmpty(values[(int)TextBoxIndices.Account])) // this seems also to check perhaps if
                                                                                // list has no element at all, provided
                                                                                // a conversion of an empty list of
                                                                                // RecordSeriaizable elements gets
                                                                                // transformed into a Null during 
                                                                                // the transformation to the string
                                                                                // array above.

                {
                    // store TextBox values in RecordSerializable and set to the 
                    // text boxes, provided that everything is OK regarding format
                    // and account number:
                    try
                    {
                        // get account-number value from TextBox
                        int accountNumber = int.Parse(
                           values[(int)TextBoxIndices.Account]);

                        // determine whether accountNumber is valid
                        if (accountNumber > 0)
                        {
                            // copy string-array values to TextBox values
                            SetTextBoxValues(values);

                        }
                        else
                        {
                            // notify user if invalid account number
                            MessageBox.Show("Invalid Account Number", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    catch (FormatException)
                    {
                        MessageBox.Show("Invalid Format", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                else
                {
                    MessageBox.Show("List is Empty!", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // ClearTextBoxes(); // clear TextBox values.  I don't think it is needed here.
            }
        }

        // Method for handling deletion of element:
        private void delete_current_element_Click(object sender, EventArgs e)
        {
            if (catalogCarEntry.Count() >= 2)
            {
                catalogCarEntry.RemoveAt(counter);

                // Now find out whether the element moved was the highest on in the list:
                if ((counter + 1) > catalogCarEntry.Count())  // I added 1 to counter due to its 0 initial value.
                {
                    counter = counter - 1;
                }
                // Otherwise the counter remains the same:

                currentRecord = catalogCarEntry[counter];
                var nowValues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                // Display the contents of the element indicated by the counter now:

                SetTextBoxValues(nowValues);
            }

            else if (catalogCarEntry.Count() == 1) // If there is only one element in the element.
            {
                catalogCarEntry.RemoveAt(counter); // Now the list should be empty and hence only contain 
                                                   // the element catalogCarEntry[0].
                MessageBox.Show("You Just Removed the Last Element!  Now the List is Empty!", "Announcement",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                counter = 0; // Counter put to zero by default.  Actually there isn't any element in an empty list.
                // In any case a good starting point for subsequent stages in the program.

                // Display empty text boxes:
                ClearTextBoxes();
            }

            else
            {
                MessageBox.Show("List Was Empty to Begin with!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                counter = 0; // counter put to zero by default.  Actually there is no elemet in an empty list.

                // Display empty text boxes:
                ClearTextBoxes();
            }


        }

        // Method for handling modification of element in question:
        private void modify_element_Click(object sender, EventArgs e)
        {

            /* Here it as about trying what the getBox Method has yielded,
             * and catching the string array, named values presumably, for
             * for format errors.
             * As for those that made it into the try block, check if the 
             * account value is an integer larger than 0.
             * 
             */

            // Now lets first get the values that are (or are not) in the boxes:
            string[] values = GetTextBoxValues();

            // Now determine if the account field (the integer number entry in the catalog) is empty:
            if (!string.IsNullOrEmpty(values[(int)TextBoxIndices.Account]))
            {
                // store TextBox values in the element of type RecordSerializable in the catalogCarEntry list:
                try
                {
                    int accountNumber = int.Parse(values[(int)TextBoxIndices.Account]);

                    // Determine whether accountNumber is valid:
                    if (accountNumber > 0) // Maybe later add a check for correctness of the format of the 'balance field' too. 
                    {
                        // Record containing TextBox values to output
                        var tempStorageRecord = new RecordSerializable(accountNumber,
                            values[(int)TextBoxIndices.First],
                            values[(int)TextBoxIndices.Last],
                            decimal.Parse(values[(int)TextBoxIndices.Balance]));

                        // Put that record as an element in its proper place within
                        // the catalogCarEntry list:

                        if (justModifyCurrentValue) // No maniplation of indexes or List elements necessary
                        {
                            if (catalogCarEntry.Count() >= 1) // Making sure the list is not empty.
                            {
                                catalogCarEntry[counter] = tempStorageRecord;

                                // Put a copy of that newly modified element and put it into a temporary record of type 
                                // RecordSerializable:
                                currentRecord = catalogCarEntry[counter];
                                var currentvalues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                                // Display the contents of the modified element in the text boxes:
                                SetTextBoxValues(currentvalues);
                            }
                            else
                            {
                                MessageBox.Show("List Is Empty! No Element to Modify", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                                counter = 0; // counter put to zero by default.  Actually there is no elemet in an empty list.

                                // Display empty text boxes:
                                ClearTextBoxes();
                            }

                        }

                        if (insertValueInCurrentLocation) // Here some maniplation of indexes or List-elements necessary
                        {
                            if (catalogCarEntry.Count() >= 1) // Making sure the list is not empty.
                            {
                                // Insert an element to the list 'catalogCarEntry' at the location specified
                                // by the current value of the counter:
                                catalogCarEntry.Insert(counter, tempStorageRecord);

                                // Put a copy of that newly inserted element and put it into a temporary record of type 
                                // RecordSerializable:
                                currentRecord = catalogCarEntry[counter];
                                var insertedcurrentvalues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                                // Display the contents of the inserted element in the text boxes:
                                SetTextBoxValues(insertedcurrentvalues);
                            }

                            else
                            {
                                // catalogCarEntry[0] = tempStorageRecord; // If the list is empty, it is transformed into one with one element.
                                
                                catalogCarEntry.Add(tempStorageRecord);

                                // Put a copy of that newly inserted element and put it into a temporary record of type 
                                // RecordSerializable:
                                currentRecord = catalogCarEntry[0];
                                var insertedcurrentvalues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                                // Display the contents of the inserted element in the text boxes:
                                SetTextBoxValues(insertedcurrentvalues);
                            }
                        }

                        if (appendValueToEndOfList) // Here an element is added to the end of the list:
                        {
                            if (catalogCarEntry.Count() >= 1) // Making sure the list is not empty.
                            {
                                // Append an element to the list 'catalogCarEntry' at the very end:
                                // by the current value of the counter:
                                catalogCarEntry.Add(tempStorageRecord);  // This appends the tempStorageRecord to the end of the list.

                                // Now display the newly appended element that is at the very end of the modified list:

                                // First get the index value of the last element which is the number of elements minus one.  
                                // (Minus one because the indexes start at zero.)
                                counter = catalogCarEntry.Count() - 1;

                                // Put a copy of that newly appended element and put it into a temporary record of type 
                                // RecordSerializable:
                                currentRecord = catalogCarEntry[counter];
                                // store RecordSerializable values in temporary string array
                                var appendedvalues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                                // Display the contents of the appended element in the text boxes:
                                SetTextBoxValues(appendedvalues);

                            }

                            else
                            {
                                // catalogCarEntry.Add(tempStorageRecord); // If the list is empty, it is transformed into one with one element
                                // consisting of the newly added record.
                                // I think you get a one element list if you do this to an empty list.

                                // I think it has been assured here that here catalogCarEntry can't be anything but an empty List.
                                // Perhaps some extra test here would be advisable:
                                catalogCarEntry.Add(tempStorageRecord);

                                // Put a copy of that newly appended element and put it into a temporary record of type 
                                // RecordSerializable:
                                currentRecord = catalogCarEntry[0];
                                var appendedvalues = new string[] {
                                    currentRecord.Account.ToString(),
                                    currentRecord.FirstName.ToString(),
                                    currentRecord.LastName.ToString(),
                                    currentRecord.Balance.ToString()
                                };

                                // Display the contents of the appended element in the text boxes:
                                SetTextBoxValues(appendedvalues);
                            }
                        }

                    }
                    else
                    {
                        // Notifying the user if the account number was invalid
                        MessageBox.Show("Invalid Account Number", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // It can be mentioned here that I think catching serialization is not relevant here.

                catch (FormatException)
                {
                    MessageBox.Show("Invalid Format", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            // Return to the default settings of in terms of which buttons are enabled and not enabled:
            step_down.Enabled = true;
            step_up.Enabled = true;
            add_element_current_location.Enabled = true;
            add_element_end_of_list.Enabled = true;
            initial_list_element.Enabled = true;
            load_from_binary_file.Enabled = true;
            save_to_binary_file.Enabled = true;
            delete_current_element.Enabled = true;
            modify_element.Enabled = true;

            // Back to the default settings as regards for which of the 3 types of modifications to implement:
            justModifyCurrentValue = true;
            insertValueInCurrentLocation = false;
            appendValueToEndOfList = false;
        }

        // Method for handling addition of element to current location:
        private void add_element_current_location_Click(object sender, EventArgs e)
        {
            justModifyCurrentValue = false;
            insertValueInCurrentLocation = true;
            appendValueToEndOfList = false;

            // var dummyRecord = new RecordSerializable(0, "", "", 0.00M); // A dummyrecord for temporary use
            // to put something into the new empty element, while waiting 
            // for a value to be entered there by the user.  I think this one is no longer needed.

            // Clear the boxes to pave way for entering new values in the field.
            ClearTextBoxes(); // clear TextBox values

            // Disable all buttons except the buttons 'Enter Modifications to Current Element'
            step_down.Enabled = false;
            step_up.Enabled = false;
            add_element_current_location.Enabled = false;
            add_element_end_of_list.Enabled = false;
            initial_list_element.Enabled = false;
            load_from_binary_file.Enabled = false;
            save_to_binary_file.Enabled = false;
            delete_current_element.Enabled = false;
            modify_element.Enabled = true;

        }

        // Method for handling addition of an element to the List:
        private void add_element_end_of_list_Click(object sender, EventArgs e)
        {
            justModifyCurrentValue = false;
            insertValueInCurrentLocation = false;
            appendValueToEndOfList = true;

            // Clear the boxes to pave way for entering new values in the field.
            ClearTextBoxes(); // clear TextBox values

            // Disable all buttons except the buttons 'Enter Modifications to Current Element'
            step_down.Enabled = false;
            step_up.Enabled = false;
            add_element_current_location.Enabled = false;
            add_element_end_of_list.Enabled = false;
            initial_list_element.Enabled = false;
            load_from_binary_file.Enabled = false;
            save_to_binary_file.Enabled = false;
            delete_current_element.Enabled = false;
            modify_element.Enabled = true;
        }

        // Method for handling going to the very first element of the list (the one with index 0) and havig is displayed:
        private void initial_list_element_Click(object sender, EventArgs e)
        {
            if (catalogCarEntry.Count() != 0) // Making sure the list is not empty.
            {
                // First set the counter to 0:
                counter = 0;

                // Put the initial element of the catalogCarEntry list
                // into the 'currentRecord' record of type RecordSerializable:
                currentRecord = catalogCarEntry[counter];
                // store RecordSerializable values in temporary string array
                var values = new string[] {
                    currentRecord.Account.ToString(),
                    currentRecord.FirstName.ToString(),
                    currentRecord.LastName.ToString(),
                    currentRecord.Balance.ToString()
                };

                // copy string-array values to TextBox values
                SetTextBoxValues(values);
            }

            else
            {
                MessageBox.Show("List Is Empty!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Display empty text boxes:
                ClearTextBoxes();
            }
        }
		// IMPORTANT!: For the 'Save' method, would I create a method almost identical to 
		// this method here (which to me looks like a 'Save As' method due to its request for
		// entering a filename), except leave out the modules
		
		// using(SaveFileDalog ...)
		// {...} // For I am not using any dialog to enter a file name.
		
		// I could probably also leave out the rest, except for the innermost nucleus,
		// namely the 'pure' try/catch block.
		
		// The challenge is, how would I determine if 'Save As' has never been entered
		// previously.
		
		// Would I perhaps put in some global toggle value, which gets toggled each time
		// a 'Save As' handler is activated?
		
		// If it turns out that a 'Save As' operation needs to be carried out, when a 
		// 'Save' is first pressed, could I call the 'Save As' event handler from within
		// the 'Save Event' handler method?
		// Would I perhaps need to create to 'Save As' methods?  One with the event handler
		// input variables for the actual button press and another one with no input variables?
		
		// Another challenge in this context, would I need to define the variables regarding
		// putting data to the file to be saved, like outputStream for example, need to be
		// defined globally at the very beginning, so that they can be accessed by both 'Save' and
		// 'Save As' event handler methods, or any methods for that matter carrying those names?
		
		// END OF IMPORTANT!
		
        // Method for having the elemets of the list output and saved to an external file:
        private void save_to_binary_file_Click(object sender, EventArgs e)
        {
            // create and show dialog box enabling user to save file:
            DialogResult result;
            string outputfileName; // Name of file to save data to.  I use outputfileName instead of
                                   // just filename in order to avoid any amibiguity.

            using (SaveFileDialog outputfileChooser = new SaveFileDialog())
            {
                outputfileChooser.CheckFileExists = false; // Let user create file.

                // retrieve the result of the dialog box:
                result = outputfileChooser.ShowDialog();
                outputfileName = outputfileChooser.FileName; // Get specified file name.
            }

            // Ensure that user clicked "OK"
            if (result == DialogResult.OK)
            {
                // Show error if user specified invalid file:
                if (string.IsNullOrEmpty(outputfileName))
                {
                    MessageBox.Show("Invalid File Name", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Save file via FileStream if user specified valid file:
                    try
                    {
                        // Open file with write access:
                        /* This is from the Fig17_09 project.  This I could implement differently
                         * using the concept of outputing a list to a serial file using suggestions 
                         * Brian sent me on December 24th.
                         */

                        outputstream = new FileStream(outputfileName, FileMode.OpenOrCreate, FileAccess.Write);
                        // This 'stream' variable, or whatever you might call it, corresponds to the variable 
                        // 'output' in the Fig17_09 project.

                        // disable Save button and enable Enter button
                        // saveButton.Enabled = false // This I could use in my adapted solution, if you will.
                        // enterButton.Enabled = true // This one might not be relevant in the adapted solution.

                        // Here comes something inspired by the so called adapted solution:

                        serializer.Serialize(outputstream, catalogCarEntry); // The catalogCarEntry is of a construction
                                                                       // List<T> and corresponds to the 'list' in the example 
                                                                       // from Brian sent to me on 24 December.
                                                                       // Brian's list is of construction 'ArrarList'.  
                                                                       // In this context I think 'Serialize' method
                                                                       // can be used for either one of thos construction types.

                    }

                    catch (IOException)
                    {
                        // notify user if file could not be opened
                        MessageBox.Show("Error opening file", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            // Finally close the file that one was outputting data to:
			// IMPORTANT!: You spoke about the program crashing because I hadn'take
			// closed the file I had saved to, but to me it looks like I have.
			// Please see here below the use of outputstram.Close():
			
			// Do I perhaps need to put this try/catch block in to the load_from_binary_file 
			// event handler too?
			// If so where should I place, presumably this same code there?
			
			// If I need to place this code in the load_from_binary_file event handler
			// method, I reckon that I would need to declare the relevant variables,
			// such as outputStram as global, so that they be accessible from the
			// load_from_binary_file event handler method as well.
			
			// END IMPORTANT!
		
			
            try
            {
                outputstream.Close(); // Close FileStream
            }
            catch (IOException)
            {
                MessageBox.Show("Cannot close file", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // I'm not putting any Exit mechanism here.  One can be found in the exit event handler.

        }

        // Method for handling loading records from an external file (presumably .SER file) into the list named 'catalogCarEntry'
        // which is for elements of type 'RecordSerializable':
        private void load_from_binary_file_Click(object sender, EventArgs e)
        {
            // Create and show dialog box enabling user to open file:
            DialogResult result; // result of OpenFileDialog.
            string inputfileName;

            using (OpenFileDialog inputfileChooser = new OpenFileDialog())
            {
                result = inputfileChooser.ShowDialog();
                inputfileName = inputfileChooser.FileName; // Get specified name.
            }

            // Ensure that user clicked "OK":
            if (result == DialogResult.OK)
            {
                ClearTextBoxes();

                // Show error if user specified invalid file:
                if (string.IsNullOrEmpty(inputfileName))
                {
                    MessageBox.Show("Invalid File Name", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Create FileStream to obtain read access to file:
                    inputstream = new FileStream(
                        inputfileName, FileMode.Open, FileAccess.Read);

                    // openButton.Enabled = false; // disaable Open File button. (Might be irrelevant here.)
                    // nextButton.Enable = true; // enable Next Record button.  (Might be irrelevant here.)

                    // In the following code stucture (which will be within this 'else' module, we will put the 
                    // contents of the 'stream' into the 'List' nameed 'catalogCarEntry'.  Of course we have 
                    // incorporated a try and catch system, in case that 'stream' contains something that 'List'
                    // doesn't like.

                    // Deserialize:
                    try
                    {
                        // List<RecordSerializable> catalogCarEntry = null; // Experimental command.
                        // catalogCarEntry = (List<RecordSerializable>)serializer.Deserialize(inputstream);
                        catalogCarEntry = (List<RecordSerializable>)serializer.Deserialize(inputstream);

                        // Show the 0th elemet of the catalogCarEntry list, so that the boxes won't 
                        // be empty after the loading procedure:
                        if (catalogCarEntry.Count() != 0) // Making sure the list is not empty.
                        {
                            // First set the counter to 0:
                            counter = 0;

                            // Put the initial element of the catalogCarEntry list
                            // into the 'currentRecord' record of type RecordSerializable:
                            currentRecord = catalogCarEntry[counter];
                            // store RecordSerializable values in temporary string array
                            var values = new string[] {
                                currentRecord.Account.ToString(),
                                currentRecord.FirstName.ToString(),
                                currentRecord.LastName.ToString(),
                                currentRecord.Balance.ToString()
                            };

                            // copy string-array values to TextBox values
                            SetTextBoxValues(values);
                        }

                        else
                        {
                            MessageBox.Show("List Is Empty!", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Display empty text boxes:
                            ClearTextBoxes();
                        }
                    }

                    catch (SerializationException)
                    {
                        inputstream?.Close(); // close FileStream
                        // openButon.Enabled = true; // Irrelevant here presumably.
                        // nextButton.Enabled = false; // Disable Next Record button.

                        ClearTextBoxes();

                        // Notify user if no RecordSerializables in file
                        MessageBox.Show("No more records in file", string.Empty,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        // Medthod for taking care of exiting the application. 
        private void exit_button_Click(object sender, EventArgs e)
        {
            // Close file
            try
            {
                outputstream?.Close(); // close Filestream.  Since I have put a mechanism
                                       // to close a file in the save file event handler just
                                       // recently this one is perhaps superfluous.
                                       // Still I leave the command here all the same.
            }
            catch(IOException)
            {
                MessageBox.Show("Cannot close file", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			
			Application.Exit();
        }
    }
}
