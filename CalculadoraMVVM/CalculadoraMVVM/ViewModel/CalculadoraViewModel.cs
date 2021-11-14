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
		string accion = "0";
		double accumulatedSum = 0;
		

		
			public CalculadoraViewModel()
		{
			ClearCommand = new Command(
				execute: () =>
				{
					HistoryString = "";
					accumulatedSum = 0;
					CurrentEntry = "0";
					
					RefreshCanExecutes();
				});

			ClearEntryCommand = new Command(
				execute: () =>
				{
					CurrentEntry = "0";
					
					RefreshCanExecutes();
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
					accion = "+";
					accumulatedSum +=value;
					CurrentEntry = accumulatedSum.ToString();
					isSumDisplayed = true;
					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return !isSumDisplayed;
				});
			RestarCommand = new Command(
				execute: () =>
				{
					double value = Double.Parse(CurrentEntry);
					HistoryString += " - ";
					accion = "-";
					if (accumulatedSum == 0)
                    {
						accumulatedSum = value;
                    }
                    else
                    {
						accumulatedSum -= value;
					}
					
					CurrentEntry = accumulatedSum.ToString();
					isSumDisplayed = true;
					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return !isSumDisplayed;
				});
			MultiplicarCommand = new Command(
				execute: () =>
				{
					double value = Double.Parse(CurrentEntry);
					HistoryString += " * ";
					accion = "*";
					accumulatedSum = value;
					CurrentEntry = accumulatedSum.ToString();
					isSumDisplayed = true;
					RefreshCanExecutes();
				},
				canExecute: () =>
				{
					return !isSumDisplayed;
				});
			DividirCommand = new Command(
				execute: () =>
				{
					double value = Double.Parse(CurrentEntry);
					HistoryString += " / ";
					accion = "/";
					accumulatedSum = value;
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
					switch (accion)
					{

						case  "+":
							
							Console.WriteLine("VALUE 1s "+value.ToString());
							Console.WriteLine("ACCUMULATE ", + accumulatedSum);
							accumulatedSum += value;
							HistoryString += "=" + accumulatedSum.ToString();
							currentEntry = "0";
							break;
						case "-":
							
							accumulatedSum -= value;
							HistoryString += "=" + accumulatedSum.ToString();
							currentEntry = "0";
							break;
						case "*":

							accumulatedSum *= value;
							HistoryString += "=" + accumulatedSum.ToString();

							break;
						case "/":

							accumulatedSum /= value;
							HistoryString += "=" + accumulatedSum.ToString();

							break;
						default:
							Console.WriteLine("Measured value is ");
							break;
					}
					
	
					RefreshCanExecutes();
					
				},
					canExecute: () =>
					{
						return CurrentEntry.Length < 16;
					});
		}

		void RefreshCanExecutes()
		{
			//	((Command)BackspaceCommand).ChangeCanExecute();
			((Command)MostrarCommand).ChangeCanExecute();
			((Command)MultiplicarCommand).ChangeCanExecute();
			((Command)DividirCommand).ChangeCanExecute();
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

		//public ICommand BackspaceCommand { private set; get; }

		public ICommand NumericCommand { private set; get; }

		public ICommand DecimalPointCommand { private set; get; }

		public ICommand AddCommand { private set; get; }
		public ICommand RestarCommand { private set; get; }

		public ICommand MultiplicarCommand { private set; get; }
		public ICommand DividirCommand { private set; get; }

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
