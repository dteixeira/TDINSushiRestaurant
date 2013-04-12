using System;

[Serializable]
public class Sushi
{
    private double _price;
    private int _quantity;
    private string _type;

    public Sushi(string type, double price)
    {
        _type = type;
        _price = price;
        _quantity = 0;
    }

    public Sushi(string type, double price, int quantity)
    {
        _type = type;
        _price = price;
        _quantity = quantity;
    }

    public double Price
    {
        get { return _price; }
    }

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    public string Type
    {
        get { return _type; }
    }
}