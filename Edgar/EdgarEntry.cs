using System;
using System.Collections.Generic;

public class Category
{
    public string label { get; set; }
    public string scheme { get; set; }
    public string term { get; set; }
}

public class Content
{
    public string type { get; set; }
    public string accession_nunber { get; set; }
    public string act { get; set; }
    public string file_number { get; set; }
    public string file_number_href { get; set; }
    public string filing_date { get; set; }
    public string filing_href { get; set; }
    public string filing_type { get; set; }
    public string film_number { get; set; }
    public string form_name { get; set; }
    public string size { get; set; }
    public string amend { get; set; }
}

public class Link
{
    public string href { get; set; }
    public string rel { get; set; }
    public string type { get; set; }
}

public class Summary
{
    public string type { get; set; }
    public string text { get; set; }
}

public class Entry
{
    public Category category { get; set; }
    public Content content { get; set; }
    public string id { get; set; }
    public Link link { get; set; }
    public Summary summary { get; set; }
    public string title { get; set; }
    public DateTime updated { get; set; }
}

public class Root
{
    public List<Entry> entry { get; set; }
}

public class EdgarEntry
{
    public Root root { get; set; }
}