﻿using System;
using System.Collections.Generic;

namespace Reizen.Models;

public partial class Boeking
{
    public int Id { get; set; }

    public int Klantid { get; set; }

    public int Reisid { get; set; }

    public DateOnly GeboektOp { get; set; }

    public int? AantalVolwassenen { get; set; }

    public int? AantalKinderen { get; set; }

    public bool AnnulatieVerzekering { get; set; }

    public virtual Klant Klant { get; set; } = null!;

    public virtual Reis Reis { get; set; } = null!;
}
