using System;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public interface IChatMessageFactory
    {
        Task<ChatMessage> FromRawString(string rawString, DateTime time);
    }
}