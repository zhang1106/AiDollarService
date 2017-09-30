using System;
using System.Collections.Generic;

public class Address
{
    public string type { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string street1 { get; set; }
    public string zip { get; set; }
    public string phone { get; set; }
}

public class Addresses
{
    public List<Address> address { get; set; }
}

public class Names
{
    public string date { get; set; }
    public string name { get; set; }
}

public class FormerlyNames
{
    public string count { get; set; }
    public Names names { get; set; }
}

public class CompanyInfo
{
    public Addresses addresses { get; set; }
    public string assigned_sic { get; set; }
    public string assigned_sic_desc { get; set; }
    public string assigned_sic_href { get; set; }
    public string assitant_director { get; set; }
    public string cik { get; set; }
    public string cik_href { get; set; }
    public string conformed_name { get; set; }
    public string fiscal_year_end { get; set; }
    public FormerlyNames formerly_names { get; set; }
    public string state_location { get; set; }
    public string state_location_href { get; set; }
    public string state_of_incorporation { get; set; }
}

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
    public CompanyInfo company_info { get; set; }
    public List<Entry> entry { get; set; }
}

public class RootObject
{
    public Root root { get; set; }
}