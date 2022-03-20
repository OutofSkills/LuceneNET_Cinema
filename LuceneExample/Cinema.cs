using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneExample
{
    public class Cinema
    {
        public List<Document> GetDocuments()
        {
            //var indexer = new LuceneIndexer(@"C:\Temp\Lucene");
            //indexer.ClearIndex();

            var list = new List<Movie>();
            list.Add(new Movie { Id = 1, Content = "The Simpsons" });
            list.Add(new Movie { Id = 2, Content = "Simpsons the Movie" });
            list.Add(new Movie { Id = 3, Content = "The Little Rascals" });
            list.Add(new Movie { Id = 4, Content = "Terminator: Salvation" });
            list.Add(new Movie { Id = 5, Content = "Terminator 3: Rise of the Machines" });
            list.Add(new Movie { Id = 6, Content = "The Terminator" });

            var documents = new List<Document>();

            foreach (var item in list)
            {
                var doc = new Document();
                doc.Add(new Field("id", item.Id.ToString(), TextField.TYPE_STORED));
                doc.Add(new Field("description", item.Content, TextField.TYPE_STORED));
                documents.Add(doc);
            }

            //indexer.BuildCompleteIndex(documents);

            return documents;
        }
    }
}
