﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int CarId { get; set; }

    public int BuyerId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal SellingPrice { get; set; }

    public string TransactionStatus { get; set; }

    public virtual User Buyer { get; set; }

    public virtual Car Car { get; set; }
}