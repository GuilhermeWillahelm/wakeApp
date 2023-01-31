using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using wakeApp.Dtos;
using wakeApp.Models;
using wakeApp.Repositories;

namespace wakeApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7099/api/") };

        public CommentDto AddComment(CommentDto comment)
        {
            if (comment == null)
            {
                return null;
            }

            string data = JsonConvert.SerializeObject(comment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "Comments/", content).Result;
            var response = _httpClient.PostAsJsonAsync<CommentDto>(_httpClient.BaseAddress + "Comments/", comment);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                return null;
            }

            return comment;
        }

        public bool DeleteComment(int id, int idUser)
        {
            //HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Comments/" + id + "/"+ idUser).Result;
            var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "Comments/" + id + "/" + idUser);
            response.Wait();

            if (response.Status != TaskStatus.RanToCompletion)
            {
                return false;
            }

            return true;
        }

        public List<CommentDto> GetCommentsPerVideos(int? idVideo)
        {
            if (idVideo == null)
            {
                return null;
            }

            //List<ViewModel> auxPosts = new List<ViewModel>();
            List<CommentDto> comments = new List<CommentDto>();
            var response = _httpClient.GetFromJsonAsync<List<CommentDto>>(_httpClient.BaseAddress + "Comments/GetCommentsPerVideo/" + idVideo);
            response.Wait();

            if (response.Status == TaskStatus.RanToCompletion)
            {
                comments = response.Result;
            }

            if (comments == null)
            {
                return null;
            }

            return comments;
        }
    }
}
