using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using System.Security.Claims;
using WebChat.Models;
using WebChat.Models.View;
using WebChat.Repositories;

namespace WebChat.Controllers
{
    [Authorize]
    public class ChatsController : Controller
    {
        private readonly TChatRepository _chatRepository;
        public ChatsController(TChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public ActionResult MyChats()
        {
            var chats = _chatRepository.LoadUserChats(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(ch => new ChatViewModel()
            {
                ChatId = ch.RowKey,
                OwnerId = ch.OwnerId,
                Title = ch.Title,
            }).ToList();
            return View("MyChats",chats);
        }
        public async Task<ActionResult> ChatAsync(string id)
        {
            var chat=await _chatRepository.GetChatById(id);
            if(chat == null)
            {
                return BadRequest();
            }
            ChatViewModel model = new ChatViewModel()
            {
                ChatId = id,
                OwnerId = chat.OwnerId,
                Title = chat.Title,
                Users = _chatRepository.GetChatUsers(chat.RowKey)
                    .Select(u => 
                    new UserViewModel() 
                    { 
                        Id = u.RowKey, 
                        Name = u.Nickname 
                    })
            };
            return View(model);
        }
        public IAsyncEnumerable<IMessageViewModel> ChatMessagesAsync(string id)
        {
            return _chatRepository.GetChatMessages(id);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatsController/Create
        [HttpPost] 
        public async Task<ActionResult> CreateAsync(TChat chat)
        {
            if(chat.OwnerId!=HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                return BadRequest();
            if (chat.Title.Length==0)
            {
                ModelState.AddModelError("Title", "Title can't be empty");
                return View(chat);
            }
            await _chatRepository.CreateChatAsync(chat);
            return Join(chat.RowKey);
        }
        [HttpGet("/Join/{id}")]
        public ActionResult Join(string id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_chatRepository.LoadUserChats(userId).Any(ch => ch.RowKey == id))
                ViewData["message"] = "You already join this chat";
            else
                _chatRepository.JoinChat(userId,id);
            return Redirect($"/Chats/Chat/{id}");
        }
    }
}
