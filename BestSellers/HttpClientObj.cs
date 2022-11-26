namespace BestSellers
{
    public class HttpClientObj
    {
        HttpClient client = new HttpClient();

        public async Task API()
        {
            HttpClientObj clientObj = new HttpClientObj();
            await clientObj.GetAPI();
        }
        private async Task GetAPI()
        {
            var json = await client.GetStringAsync(
            "https://jsonplaceholder.typicode.com/todos");

            Console.Write(json);
        }
    }
}
