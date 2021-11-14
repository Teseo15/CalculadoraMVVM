using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace CalculadoraMVVM.ViewModel
{
    public class CalculadoraViewModel : ViewModelBase
    {
        string currentEntry = "0";
        string historyString = "";
        bool isSumDisplayed = false;
        double accumulatedSum = 0;

        public CalculadoraViewModel()
		{
			ClearCommand = new Command(
				execute: () =>
				{
					HistoryString = "";
					accumulatedSum = 0;
					CurrentEntry = "0";
					isSumDisplayed = false;
					RefreshCanExecutes();
				});

			ClearEntryCommand = new Command(
				execute: () =>
				{
					CurrentEntry = "0";
					isSumDisplayed = false;
					RefreshCanExecutes();
				});

			BackspaceCommand = new Command(
				execute: () =>
				{
					CurrentEntry = CurrentEntry.Substring(0, CurrentEntry.Length - 1);

					if (CurrentEntry.Length == 0)
					{
						CurrentEntry = "0";
					}

					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return !isSumDisplayed && (CurrentEntry.Length > 1 || CurrentEntry[0] != '0');
				});

			NumericCommand = new Command<string>(
				execute: (string parameter) =>
				{
					if (isSumDisplayed || CurrentEntry == "0")
						CurrentEntry = parameter;
					else
						CurrentEntry += parameter;
					HistoryString += parameter.ToString();
					isSumDisplayed = false;
					RefreshCanExecutes();
				},
				canExecute: (string parameter) =>
				{
					return isSumDisplayed || CurrentEntry.Length < 16;
				});



			DecimalPointCommand = new Command(
				execute: () =>
				{
					if (isSumDisplayed)
						CurrentEntry = "0.";
					else
						CurrentEntry += ".";

					isSumDisplayed = false;
					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return isSumDisplayed || !CurrentEntry.Contains(".");
				});
			
			AddCommand = new Command(
				execute: () =>
				{
					double value = Double.Parse(CurrentEntry);
					HistoryString +=" + ";
					accumulatedSum +=value;
					CurrentEntry = accumulatedSum.ToString();
					
					isSumDisplayed = true;
					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return !isSumDisplayed;
				});
			MostrarCommand = new Command(
				execute: () =>
				{
					double value = Double.Parse(CurrentEntry);

					accumulatedSum += value;
					HistoryString += "=" + accumulatedSum.ToString();
					isSumDisplayed = true;
					RefreshCanExecutes();
					
				},
					canExecute: () =>
					{
						return isSumDisplayed || CurrentEntry.Length < 16;
					});
		}

		void RefreshCanExecutes()
		{
			((Command)BackspaceCommand).ChangeCanExecute();
			((Command)NumericCommand).ChangeCanExecute();
			((Command)DecimalPointCommand).ChangeCanExecute();
			((Command)AddCommand).ChangeCanExecute();
		}

		public string CurrentEntry
		{
			private set
			{
				if (currentEntry!= value)
				{
					currentEntry = value;
					OnPropertyChanged();
				}
			}
			get { return currentEntry; }
		}

		public string HistoryString
		{
			private set
			{
				if (historyString!= value)
				{
					historyString = value;
					OnPropertyChanged();
				}
			}
			get { return historyString; }
		}

		public ICommand ClearCommand { private set; get; }

		public ICommand ClearEntryCommand { private set; get; }

		public ICommand BackspaceCommand { private set; get; }

		public ICommand NumericCommand { private set; get; }

		public ICommand DecimalPointCommand { private set; get; }

		public ICommand AddCommand { private set; get; }

		public ICommand MostrarCommand { private set; get; }

		public void SaveState(IDictionary<string, object> dictionary)
		{
			dictionary["CurrentEntry"] = CurrentEntry;
			dictionary["HistoryString"] = HistoryString;
			dictionary["isSumDisplayed"] = isSumDisplayed;
			dictionary["accumulatedSum"] = accumulatedSum;
		}

		public void RestoreState(IDictionary<string, object> dictionary)
		{
			CurrentEntry = GetDictionaryEntry(dictionary, "CurrentEntry", "0");
			HistoryString = GetDictionaryEntry(dictionary, "HistoryString", "");
			isSumDisplayed = GetDictionaryEntry(dictionary, "isSumDisplayed", true);
			accumulatedSum = GetDictionaryEntry(dictionary, "accumulatedSum", 0.0);

			RefreshCanExecutes();
		}

		public T GetDictionaryEntry<T>(IDictionary<string, object> dictionary,string key, T defaultValue)
		{
			if (dictionary.ContainsKey(key))
				return (T)dictionary[key];

			return defaultValue;
		}
	}
}
