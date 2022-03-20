using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers.Classic;

public class LuceneIndexer
{
    private Analyzer _analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
    private string _indexPath;
    private Lucene.Net.Store.Directory _indexDirectory;
    private IndexWriter _indexWriter;
    private IndexWriterConfig _indexWriterConfig;


    public LuceneIndexer(string indexPath)
    {
        this._indexPath = indexPath;
        _indexDirectory = new MMapDirectory(new System.IO.DirectoryInfo(_indexPath));
        _indexWriterConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, _analyzer);
    }

    public void BuildCompleteIndex(IEnumerable<Document> documents)
    {
        if(_indexWriter is null)
            _indexWriter = new IndexWriter(_indexDirectory, _indexWriterConfig);

        foreach (var doc in documents)
        {
            _indexWriter.AddDocument(doc);
        }

        _indexWriter.Flush(true, true);
        _indexWriter.Commit();
        _indexWriter.Dispose();
    }

    public int UpdateIndex(IEnumerable<Document> documents)
    {
        throw new NotImplementedException();
    }

    public void ClearIndex()
    {
        _indexWriter = new IndexWriter(_indexDirectory, _indexWriterConfig);
        _indexWriter.DeleteAll();
        _indexWriter.Commit();
        //_indexWriter.Dispose();
    }


    //Single field search
    public IEnumerable<Document> Search(string searchTerm, string searchField, int limit)
    {
        DirectoryReader ireader = DirectoryReader.Open(_indexDirectory);
        var searcher = new IndexSearcher(ireader);
        var parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, searchField, _analyzer);
        var query = parser.Parse(searchTerm);
        var hits = searcher.Search(query, limit).ScoreDocs;

        var documents = new List<Document>();
        foreach (var hit in hits)
        {
            documents.Add(searcher.Doc(hit.Doc));
        }

        //_analyzer.Close();
        //searcher.Dispose();
        return documents;
    }

    //Allows multiple field searches
    public IEnumerable<Document> Search(string searchTerm, string[] searchFields, int limit)
    {
        var searcher = new IndexSearcher(IndexReader.Open(_indexDirectory));
        var parser = new MultiFieldQueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_30, searchFields, _analyzer);
        var query = parser.Parse(searchTerm);
        var hits = searcher.Search(query, limit).ScoreDocs;

        var documents = new List<Document>();
        foreach (var hit in hits)
        {
            documents.Add(searcher.Doc(hit.Doc));
        }

        //_analyzer.Close();
        //searcher.Dispose();
        return documents;
    }

}