
using otp.Services;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using otp.Model2;

namespace otp.ViewModels
{
    public class ChatViewModel : FreshBasePageModel
    {



        // private readonly IChatService chatService;
        private readonly IOpenAIService OpenAIService;

        string chatText;
        public string ChatText
        {
            get => chatText;
            set
            {

                chatText = value;
                RaisePropertyChanged("ChatText");
            }
        }


        public ObservableCollection<Chat> ChatList { get; } = new ObservableCollection<Chat>();

        public ICommand SendCommand { get; }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {

                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        public ChatViewModel(IOpenAIService OpenAIService)
        {
            this.OpenAIService = OpenAIService;
            SendCommand = new Command(async () => await Send());
            ChatList.Clear();
        }

        private async Task Send()
        {
            if (string.IsNullOrEmpty(ChatText)) return;
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                string question = ChatText;
                ChatText = string.Empty;

                ChatList.Add(new Chat() { Id = Guid.NewGuid().ToString(), User = "Me", Message = question, IsIncoming = false, Date = DateTime.Now });

                await Task.Delay(5000); // Introduce a 5-second delay

                Task<string> responseTask = OpenAIService.AskQuestion(question);
                await responseTask;

                string response = responseTask.Result;

                if (!string.IsNullOrEmpty(response))
                {
                    ChatList.Add(new Chat() { Id = Guid.NewGuid().ToString(), User = "AI", Message = response, IsIncoming = true, Date = DateTime.Now });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}


