// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Pluralization.EnglishPluralizationService
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Design.PluralizationServices;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.Entity.Infrastructure.Pluralization
{
  /// <summary>
  /// Default pluralization service implementation to be used by Entity Framework. This pluralization
  /// service is based on English locale.
  /// </summary>
  public sealed class EnglishPluralizationService : IPluralizationService
  {
    private readonly BidirectionalDictionary<string, string> _userDictionary;
    private readonly StringBidirectionalDictionary _irregularPluralsPluralizationService;
    private readonly StringBidirectionalDictionary _assimilatedClassicalInflectionPluralizationService;
    private readonly StringBidirectionalDictionary _oSuffixPluralizationService;
    private readonly StringBidirectionalDictionary _classicalInflectionPluralizationService;
    private readonly StringBidirectionalDictionary _irregularVerbPluralizationService;
    private readonly StringBidirectionalDictionary _wordsEndingWithSePluralizationService;
    private readonly StringBidirectionalDictionary _wordsEndingWithSisPluralizationService;
    private readonly List<string> _knownSingluarWords;
    private readonly List<string> _knownPluralWords;
    private readonly CultureInfo _culture = new CultureInfo("en-US");
    private readonly string[] _uninflectiveSuffixes = new string[7]
    {
      "fish",
      "ois",
      "sheep",
      "deer",
      "pos",
      "itis",
      "ism"
    };
    private readonly string[] _uninflectiveWords = new string[84]
    {
      "bison",
      "flounder",
      "pliers",
      "bream",
      "gallows",
      "proceedings",
      "breeches",
      "graffiti",
      "rabies",
      "britches",
      "headquarters",
      "salmon",
      "carp",
      "herpes",
      "scissors",
      "chassis",
      "high-jinks",
      "sea-bass",
      "clippers",
      "homework",
      "series",
      "cod",
      "innings",
      "shears",
      "contretemps",
      "jackanapes",
      "species",
      "corps",
      "mackerel",
      "swine",
      "debris",
      "measles",
      "trout",
      "diabetes",
      "mews",
      "tuna",
      "djinn",
      "mumps",
      "whiting",
      "eland",
      "news",
      "wildebeest",
      "elk",
      "pincers",
      "police",
      "hair",
      "ice",
      "chaos",
      "milk",
      "cotton",
      "corn",
      "millet",
      "hay",
      "pneumonoultramicroscopicsilicovolcanoconiosis",
      "information",
      "rice",
      "tobacco",
      "aircraft",
      "rabies",
      "scabies",
      "diabetes",
      "traffic",
      "cotton",
      "corn",
      "millet",
      "rice",
      "hay",
      "hemp",
      "tobacco",
      "cabbage",
      "okra",
      "broccoli",
      "asparagus",
      "lettuce",
      "beef",
      "pork",
      "venison",
      "bison",
      "mutton",
      "cattle",
      "offspring",
      "molasses",
      "shambles",
      "shingles"
    };
    private readonly Dictionary<string, string> _irregularVerbList = new Dictionary<string, string>()
    {
      {
        "am",
        "are"
      },
      {
        "are",
        "are"
      },
      {
        "is",
        "are"
      },
      {
        "was",
        "were"
      },
      {
        "were",
        "were"
      },
      {
        "has",
        "have"
      },
      {
        "have",
        "have"
      }
    };
    private readonly List<string> _pronounList = new List<string>()
    {
      "I",
      "we",
      "you",
      "he",
      "she",
      "they",
      "it",
      "me",
      "us",
      "him",
      "her",
      "them",
      "myself",
      "ourselves",
      "yourself",
      "himself",
      "herself",
      "itself",
      "oneself",
      "oneselves",
      "my",
      "our",
      "your",
      "his",
      "their",
      "its",
      "mine",
      "yours",
      "hers",
      "theirs",
      "this",
      "that",
      "these",
      "those",
      "all",
      "another",
      "any",
      "anybody",
      "anyone",
      "anything",
      "both",
      "each",
      "other",
      "either",
      "everyone",
      "everybody",
      "everything",
      "most",
      "much",
      "nothing",
      "nobody",
      "none",
      "one",
      "others",
      "some",
      "somebody",
      "someone",
      "something",
      "what",
      "whatever",
      "which",
      "whichever",
      "who",
      "whoever",
      "whom",
      "whomever",
      "whose"
    };
    private readonly Dictionary<string, string> _irregularPluralsList = new Dictionary<string, string>()
    {
      {
        "brother",
        "brothers"
      },
      {
        "child",
        "children"
      },
      {
        "cow",
        "cows"
      },
      {
        "ephemeris",
        "ephemerides"
      },
      {
        "genie",
        "genies"
      },
      {
        "money",
        "moneys"
      },
      {
        "mongoose",
        "mongooses"
      },
      {
        "mythos",
        "mythoi"
      },
      {
        "octopus",
        "octopuses"
      },
      {
        "ox",
        "oxen"
      },
      {
        "soliloquy",
        "soliloquies"
      },
      {
        "trilby",
        "trilbys"
      },
      {
        "crisis",
        "crises"
      },
      {
        "synopsis",
        "synopses"
      },
      {
        "rose",
        "roses"
      },
      {
        "gas",
        "gases"
      },
      {
        "bus",
        "buses"
      },
      {
        "axis",
        "axes"
      },
      {
        "memo",
        "memos"
      },
      {
        "casino",
        "casinos"
      },
      {
        "silo",
        "silos"
      },
      {
        "stereo",
        "stereos"
      },
      {
        "studio",
        "studios"
      },
      {
        "lens",
        "lenses"
      },
      {
        "alias",
        "aliases"
      },
      {
        "pie",
        "pies"
      },
      {
        "corpus",
        "corpora"
      },
      {
        "viscus",
        "viscera"
      },
      {
        "hippopotamus",
        "hippopotami"
      },
      {
        "trace",
        "traces"
      },
      {
        "person",
        "people"
      },
      {
        "chilli",
        "chillies"
      },
      {
        "analysis",
        "analyses"
      },
      {
        "basis",
        "bases"
      },
      {
        "neurosis",
        "neuroses"
      },
      {
        "oasis",
        "oases"
      },
      {
        "synthesis",
        "syntheses"
      },
      {
        "thesis",
        "theses"
      },
      {
        "pneumonoultramicroscopicsilicovolcanoconiosis",
        "pneumonoultramicroscopicsilicovolcanoconioses"
      },
      {
        "status",
        "statuses"
      },
      {
        "prospectus",
        "prospectuses"
      },
      {
        "change",
        "changes"
      },
      {
        "lie",
        "lies"
      },
      {
        "calorie",
        "calories"
      },
      {
        "freebie",
        "freebies"
      },
      {
        "case",
        "cases"
      },
      {
        "house",
        "houses"
      },
      {
        "valve",
        "valves"
      },
      {
        "cloth",
        "clothes"
      }
    };
    private readonly Dictionary<string, string> _assimilatedClassicalInflectionList = new Dictionary<string, string>()
    {
      {
        "alumna",
        "alumnae"
      },
      {
        "alga",
        "algae"
      },
      {
        "vertebra",
        "vertebrae"
      },
      {
        "codex",
        "codices"
      },
      {
        "murex",
        "murices"
      },
      {
        "silex",
        "silices"
      },
      {
        "aphelion",
        "aphelia"
      },
      {
        "hyperbaton",
        "hyperbata"
      },
      {
        "perihelion",
        "perihelia"
      },
      {
        "asyndeton",
        "asyndeta"
      },
      {
        "noumenon",
        "noumena"
      },
      {
        "phenomenon",
        "phenomena"
      },
      {
        "criterion",
        "criteria"
      },
      {
        "organon",
        "organa"
      },
      {
        "prolegomenon",
        "prolegomena"
      },
      {
        "agendum",
        "agenda"
      },
      {
        "datum",
        "data"
      },
      {
        "extremum",
        "extrema"
      },
      {
        "bacterium",
        "bacteria"
      },
      {
        "desideratum",
        "desiderata"
      },
      {
        "stratum",
        "strata"
      },
      {
        "candelabrum",
        "candelabra"
      },
      {
        "erratum",
        "errata"
      },
      {
        "ovum",
        "ova"
      },
      {
        "forum",
        "fora"
      },
      {
        "addendum",
        "addenda"
      },
      {
        "stadium",
        "stadia"
      },
      {
        "automaton",
        "automata"
      },
      {
        "polyhedron",
        "polyhedra"
      }
    };
    private readonly Dictionary<string, string> _oSuffixList = new Dictionary<string, string>()
    {
      {
        "albino",
        "albinos"
      },
      {
        "generalissimo",
        "generalissimos"
      },
      {
        "manifesto",
        "manifestos"
      },
      {
        "archipelago",
        "archipelagos"
      },
      {
        "ghetto",
        "ghettos"
      },
      {
        "medico",
        "medicos"
      },
      {
        "armadillo",
        "armadillos"
      },
      {
        "guano",
        "guanos"
      },
      {
        "octavo",
        "octavos"
      },
      {
        "commando",
        "commandos"
      },
      {
        "inferno",
        "infernos"
      },
      {
        "photo",
        "photos"
      },
      {
        "ditto",
        "dittos"
      },
      {
        "jumbo",
        "jumbos"
      },
      {
        "pro",
        "pros"
      },
      {
        "dynamo",
        "dynamos"
      },
      {
        "lingo",
        "lingos"
      },
      {
        "quarto",
        "quartos"
      },
      {
        "embryo",
        "embryos"
      },
      {
        "lumbago",
        "lumbagos"
      },
      {
        "rhino",
        "rhinos"
      },
      {
        "fiasco",
        "fiascos"
      },
      {
        "magneto",
        "magnetos"
      },
      {
        "stylo",
        "stylos"
      }
    };
    private readonly Dictionary<string, string> _classicalInflectionList = new Dictionary<string, string>()
    {
      {
        "stamen",
        "stamina"
      },
      {
        "foramen",
        "foramina"
      },
      {
        "lumen",
        "lumina"
      },
      {
        "anathema",
        "anathemata"
      },
      {
        "enema",
        "enemata"
      },
      {
        "oedema",
        "oedemata"
      },
      {
        "bema",
        "bemata"
      },
      {
        "enigma",
        "enigmata"
      },
      {
        "sarcoma",
        "sarcomata"
      },
      {
        "carcinoma",
        "carcinomata"
      },
      {
        "gumma",
        "gummata"
      },
      {
        "schema",
        "schemata"
      },
      {
        "charisma",
        "charismata"
      },
      {
        "lemma",
        "lemmata"
      },
      {
        "soma",
        "somata"
      },
      {
        "diploma",
        "diplomata"
      },
      {
        "lymphoma",
        "lymphomata"
      },
      {
        "stigma",
        "stigmata"
      },
      {
        "dogma",
        "dogmata"
      },
      {
        "magma",
        "magmata"
      },
      {
        "stoma",
        "stomata"
      },
      {
        "drama",
        "dramata"
      },
      {
        "melisma",
        "melismata"
      },
      {
        "trauma",
        "traumata"
      },
      {
        "edema",
        "edemata"
      },
      {
        "miasma",
        "miasmata"
      },
      {
        "abscissa",
        "abscissae"
      },
      {
        "formula",
        "formulae"
      },
      {
        "medusa",
        "medusae"
      },
      {
        "amoeba",
        "amoebae"
      },
      {
        "hydra",
        "hydrae"
      },
      {
        "nebula",
        "nebulae"
      },
      {
        "antenna",
        "antennae"
      },
      {
        "hyperbola",
        "hyperbolae"
      },
      {
        "nova",
        "novae"
      },
      {
        "aurora",
        "aurorae"
      },
      {
        "lacuna",
        "lacunae"
      },
      {
        "parabola",
        "parabolae"
      },
      {
        "apex",
        "apices"
      },
      {
        "latex",
        "latices"
      },
      {
        "vertex",
        "vertices"
      },
      {
        "cortex",
        "cortices"
      },
      {
        "pontifex",
        "pontifices"
      },
      {
        "vortex",
        "vortices"
      },
      {
        "index",
        "indices"
      },
      {
        "simplex",
        "simplices"
      },
      {
        "iris",
        "irides"
      },
      {
        "clitoris",
        "clitorides"
      },
      {
        "alto",
        "alti"
      },
      {
        "contralto",
        "contralti"
      },
      {
        "soprano",
        "soprani"
      },
      {
        "basso",
        "bassi"
      },
      {
        "crescendo",
        "crescendi"
      },
      {
        "tempo",
        "tempi"
      },
      {
        "canto",
        "canti"
      },
      {
        "solo",
        "soli"
      },
      {
        "aquarium",
        "aquaria"
      },
      {
        "interregnum",
        "interregna"
      },
      {
        "quantum",
        "quanta"
      },
      {
        "compendium",
        "compendia"
      },
      {
        "lustrum",
        "lustra"
      },
      {
        "rostrum",
        "rostra"
      },
      {
        "consortium",
        "consortia"
      },
      {
        "maximum",
        "maxima"
      },
      {
        "spectrum",
        "spectra"
      },
      {
        "cranium",
        "crania"
      },
      {
        "medium",
        "media"
      },
      {
        "speculum",
        "specula"
      },
      {
        "curriculum",
        "curricula"
      },
      {
        "memorandum",
        "memoranda"
      },
      {
        "stadium",
        "stadia"
      },
      {
        "dictum",
        "dicta"
      },
      {
        "millenium",
        "millenia"
      },
      {
        "trapezium",
        "trapezia"
      },
      {
        "emporium",
        "emporia"
      },
      {
        "minimum",
        "minima"
      },
      {
        "ultimatum",
        "ultimata"
      },
      {
        "enconium",
        "enconia"
      },
      {
        "momentum",
        "momenta"
      },
      {
        "vacuum",
        "vacua"
      },
      {
        "gymnasium",
        "gymnasia"
      },
      {
        "optimum",
        "optima"
      },
      {
        "velum",
        "vela"
      },
      {
        "honorarium",
        "honoraria"
      },
      {
        "phylum",
        "phyla"
      },
      {
        "focus",
        "foci"
      },
      {
        "nimbus",
        "nimbi"
      },
      {
        "succubus",
        "succubi"
      },
      {
        "fungus",
        "fungi"
      },
      {
        "nucleolus",
        "nucleoli"
      },
      {
        "torus",
        "tori"
      },
      {
        "genius",
        "genii"
      },
      {
        "radius",
        "radii"
      },
      {
        "umbilicus",
        "umbilici"
      },
      {
        "incubus",
        "incubi"
      },
      {
        "stylus",
        "styli"
      },
      {
        "uterus",
        "uteri"
      },
      {
        "stimulus",
        "stimuli"
      },
      {
        "apparatus",
        "apparatus"
      },
      {
        "impetus",
        "impetus"
      },
      {
        "prospectus",
        "prospectus"
      },
      {
        "cantus",
        "cantus"
      },
      {
        "nexus",
        "nexus"
      },
      {
        "sinus",
        "sinus"
      },
      {
        "coitus",
        "coitus"
      },
      {
        "plexus",
        "plexus"
      },
      {
        "status",
        "status"
      },
      {
        "hiatus",
        "hiatus"
      },
      {
        "afreet",
        "afreeti"
      },
      {
        "afrit",
        "afriti"
      },
      {
        "efreet",
        "efreeti"
      },
      {
        "cherub",
        "cherubim"
      },
      {
        "goy",
        "goyim"
      },
      {
        "seraph",
        "seraphim"
      },
      {
        "alumnus",
        "alumni"
      }
    };
    private readonly List<string> _knownConflictingPluralList = new List<string>()
    {
      "they",
      "them",
      "their",
      "have",
      "were",
      "yourself",
      "are"
    };
    private readonly Dictionary<string, string> _wordsEndingWithSeList = new Dictionary<string, string>()
    {
      {
        "house",
        "houses"
      },
      {
        "case",
        "cases"
      },
      {
        "enterprise",
        "enterprises"
      },
      {
        "purchase",
        "purchases"
      },
      {
        "surprise",
        "surprises"
      },
      {
        "release",
        "releases"
      },
      {
        "disease",
        "diseases"
      },
      {
        "promise",
        "promises"
      },
      {
        "refuse",
        "refuses"
      },
      {
        "whose",
        "whoses"
      },
      {
        "phase",
        "phases"
      },
      {
        "noise",
        "noises"
      },
      {
        "nurse",
        "nurses"
      },
      {
        "rose",
        "roses"
      },
      {
        "franchise",
        "franchises"
      },
      {
        "supervise",
        "supervises"
      },
      {
        "farmhouse",
        "farmhouses"
      },
      {
        "suitcase",
        "suitcases"
      },
      {
        "recourse",
        "recourses"
      },
      {
        "impulse",
        "impulses"
      },
      {
        "license",
        "licenses"
      },
      {
        "diocese",
        "dioceses"
      },
      {
        "excise",
        "excises"
      },
      {
        "demise",
        "demises"
      },
      {
        "blouse",
        "blouses"
      },
      {
        "bruise",
        "bruises"
      },
      {
        "misuse",
        "misuses"
      },
      {
        "curse",
        "curses"
      },
      {
        "prose",
        "proses"
      },
      {
        "purse",
        "purses"
      },
      {
        "goose",
        "gooses"
      },
      {
        "tease",
        "teases"
      },
      {
        "poise",
        "poises"
      },
      {
        "vase",
        "vases"
      },
      {
        "fuse",
        "fuses"
      },
      {
        "muse",
        "muses"
      },
      {
        "slaughterhouse",
        "slaughterhouses"
      },
      {
        "clearinghouse",
        "clearinghouses"
      },
      {
        "endonuclease",
        "endonucleases"
      },
      {
        "steeplechase",
        "steeplechases"
      },
      {
        "metamorphose",
        "metamorphoses"
      },
      {
        "intercourse",
        "intercourses"
      },
      {
        "commonsense",
        "commonsenses"
      },
      {
        "intersperse",
        "intersperses"
      },
      {
        "merchandise",
        "merchandises"
      },
      {
        "phosphatase",
        "phosphatases"
      },
      {
        "summerhouse",
        "summerhouses"
      },
      {
        "watercourse",
        "watercourses"
      },
      {
        "catchphrase",
        "catchphrases"
      },
      {
        "compromise",
        "compromises"
      },
      {
        "greenhouse",
        "greenhouses"
      },
      {
        "lighthouse",
        "lighthouses"
      },
      {
        "paraphrase",
        "paraphrases"
      },
      {
        "mayonnaise",
        "mayonnaises"
      },
      {
        "racecourse",
        "racecourses"
      },
      {
        "apocalypse",
        "apocalypses"
      },
      {
        "courthouse",
        "courthouses"
      },
      {
        "powerhouse",
        "powerhouses"
      },
      {
        "storehouse",
        "storehouses"
      },
      {
        "glasshouse",
        "glasshouses"
      },
      {
        "hypotenuse",
        "hypotenuses"
      },
      {
        "peroxidase",
        "peroxidases"
      },
      {
        "pillowcase",
        "pillowcases"
      },
      {
        "roundhouse",
        "roundhouses"
      },
      {
        "streetwise",
        "streetwises"
      },
      {
        "expertise",
        "expertises"
      },
      {
        "discourse",
        "discourses"
      },
      {
        "warehouse",
        "warehouses"
      },
      {
        "staircase",
        "staircases"
      },
      {
        "workhouse",
        "workhouses"
      },
      {
        "briefcase",
        "briefcases"
      },
      {
        "clubhouse",
        "clubhouses"
      },
      {
        "clockwise",
        "clockwises"
      },
      {
        "concourse",
        "concourses"
      },
      {
        "playhouse",
        "playhouses"
      },
      {
        "turquoise",
        "turquoises"
      },
      {
        "boathouse",
        "boathouses"
      },
      {
        "cellulose",
        "celluloses"
      },
      {
        "epitomise",
        "epitomises"
      },
      {
        "gatehouse",
        "gatehouses"
      },
      {
        "grandiose",
        "grandioses"
      },
      {
        "menopause",
        "menopauses"
      },
      {
        "penthouse",
        "penthouses"
      },
      {
        "racehorse",
        "racehorses"
      },
      {
        "transpose",
        "transposes"
      },
      {
        "almshouse",
        "almshouses"
      },
      {
        "customise",
        "customises"
      },
      {
        "footloose",
        "footlooses"
      },
      {
        "galvanise",
        "galvanises"
      },
      {
        "princesse",
        "princesses"
      },
      {
        "universe",
        "universes"
      },
      {
        "workhorse",
        "workhorses"
      }
    };
    private readonly Dictionary<string, string> _wordsEndingWithSisList = new Dictionary<string, string>()
    {
      {
        "analysis",
        "analyses"
      },
      {
        "crisis",
        "crises"
      },
      {
        "basis",
        "bases"
      },
      {
        "atherosclerosis",
        "atheroscleroses"
      },
      {
        "electrophoresis",
        "electrophoreses"
      },
      {
        "psychoanalysis",
        "psychoanalyses"
      },
      {
        "photosynthesis",
        "photosyntheses"
      },
      {
        "amniocentesis",
        "amniocenteses"
      },
      {
        "metamorphosis",
        "metamorphoses"
      },
      {
        "toxoplasmosis",
        "toxoplasmoses"
      },
      {
        "endometriosis",
        "endometrioses"
      },
      {
        "tuberculosis",
        "tuberculoses"
      },
      {
        "pathogenesis",
        "pathogeneses"
      },
      {
        "osteoporosis",
        "osteoporoses"
      },
      {
        "parenthesis",
        "parentheses"
      },
      {
        "anastomosis",
        "anastomoses"
      },
      {
        "peristalsis",
        "peristalses"
      },
      {
        "hypothesis",
        "hypotheses"
      },
      {
        "antithesis",
        "antitheses"
      },
      {
        "apotheosis",
        "apotheoses"
      },
      {
        "thrombosis",
        "thromboses"
      },
      {
        "diagnosis",
        "diagnoses"
      },
      {
        "synthesis",
        "syntheses"
      },
      {
        "paralysis",
        "paralyses"
      },
      {
        "prognosis",
        "prognoses"
      },
      {
        "cirrhosis",
        "cirrhoses"
      },
      {
        "sclerosis",
        "scleroses"
      },
      {
        "psychosis",
        "psychoses"
      },
      {
        "apoptosis",
        "apoptoses"
      },
      {
        "symbiosis",
        "symbioses"
      }
    };

    /// <summary>
    /// Constructs a new  instance  of default pluralization service
    /// used in Entity Framework.
    /// </summary>
    public EnglishPluralizationService()
    {
      this._userDictionary = new BidirectionalDictionary<string, string>();
      this._irregularPluralsPluralizationService = new StringBidirectionalDictionary(this._irregularPluralsList);
      this._assimilatedClassicalInflectionPluralizationService = new StringBidirectionalDictionary(this._assimilatedClassicalInflectionList);
      this._oSuffixPluralizationService = new StringBidirectionalDictionary(this._oSuffixList);
      this._classicalInflectionPluralizationService = new StringBidirectionalDictionary(this._classicalInflectionList);
      this._wordsEndingWithSePluralizationService = new StringBidirectionalDictionary(this._wordsEndingWithSeList);
      this._wordsEndingWithSisPluralizationService = new StringBidirectionalDictionary(this._wordsEndingWithSisList);
      this._irregularVerbPluralizationService = new StringBidirectionalDictionary(this._irregularVerbList);
      this._knownSingluarWords = new List<string>(this._irregularPluralsList.Keys.Concat<string>((IEnumerable<string>) this._assimilatedClassicalInflectionList.Keys).Concat<string>((IEnumerable<string>) this._oSuffixList.Keys).Concat<string>((IEnumerable<string>) this._classicalInflectionList.Keys).Concat<string>((IEnumerable<string>) this._irregularVerbList.Keys).Concat<string>((IEnumerable<string>) this._uninflectiveWords).Except<string>((IEnumerable<string>) this._knownConflictingPluralList));
      this._knownPluralWords = new List<string>(this._irregularPluralsList.Values.Concat<string>((IEnumerable<string>) this._assimilatedClassicalInflectionList.Values).Concat<string>((IEnumerable<string>) this._oSuffixList.Values).Concat<string>((IEnumerable<string>) this._classicalInflectionList.Values).Concat<string>((IEnumerable<string>) this._irregularVerbList.Values).Concat<string>((IEnumerable<string>) this._uninflectiveWords));
    }

    /// <summary>
    /// Constructs a new  instance  of default pluralization service
    /// used in Entity Framework.
    /// </summary>
    /// <param name="userDictionaryEntries">
    ///     A collection of user dictionary entries to be used by this service.These inputs
    ///     can  customize the service according the user needs.
    /// </param>
    public EnglishPluralizationService(
      IEnumerable<CustomPluralizationEntry> userDictionaryEntries)
      : this()
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<CustomPluralizationEntry>>(userDictionaryEntries, nameof (userDictionaryEntries));
      userDictionaryEntries.Each<CustomPluralizationEntry>((Action<CustomPluralizationEntry>) (entry => this._userDictionary.AddValue(entry.Singular, entry.Plural)));
    }

    /// <summary>Returns the plural form of the specified word.</summary>
    /// <returns>The plural form of the input parameter.</returns>
    /// <param name="word">The word to be made plural.</param>
    public string Pluralize(string word) => EnglishPluralizationService.Capitalize(word, new Func<string, string>(this.InternalPluralize));

    private string InternalPluralize(string word)
    {
      if (this._userDictionary.ExistsInFirst(word))
        return this._userDictionary.GetSecondValue(word);
      if (this.IsNoOpWord(word))
        return word;
      string prefixWord;
      string suffixWord = EnglishPluralizationService.GetSuffixWord(word, out prefixWord);
      if (this.IsNoOpWord(suffixWord) || this.IsUninflective(suffixWord) || (this._knownPluralWords.Contains(suffixWord.ToLowerInvariant()) || this.IsPlural(suffixWord)))
        return prefixWord + suffixWord;
      if (this._irregularPluralsPluralizationService.ExistsInFirst(suffixWord))
        return prefixWord + this._irregularPluralsPluralizationService.GetSecondValue(suffixWord);
      string word1 = suffixWord;
      List<string> stringList1 = new List<string>();
      stringList1.Add("man");
      CultureInfo culture1 = this._culture;
      string str;
      ref string local1 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word1, (IEnumerable<string>) stringList1, (Func<string, string>) (s => s.Remove(s.Length - 2, 2) + "en"), culture1, out local1))
        return prefixWord + str;
      string word2 = suffixWord;
      List<string> stringList2 = new List<string>();
      stringList2.Add("louse");
      stringList2.Add("mouse");
      CultureInfo culture2 = this._culture;
      ref string local2 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word2, (IEnumerable<string>) stringList2, (Func<string, string>) (s => s.Remove(s.Length - 4, 4) + "ice"), culture2, out local2))
        return prefixWord + str;
      string word3 = suffixWord;
      List<string> stringList3 = new List<string>();
      stringList3.Add("tooth");
      CultureInfo culture3 = this._culture;
      ref string local3 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word3, (IEnumerable<string>) stringList3, (Func<string, string>) (s => s.Remove(s.Length - 4, 4) + "eeth"), culture3, out local3))
        return prefixWord + str;
      string word4 = suffixWord;
      List<string> stringList4 = new List<string>();
      stringList4.Add("goose");
      CultureInfo culture4 = this._culture;
      ref string local4 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word4, (IEnumerable<string>) stringList4, (Func<string, string>) (s => s.Remove(s.Length - 4, 4) + "eese"), culture4, out local4))
        return prefixWord + str;
      string word5 = suffixWord;
      List<string> stringList5 = new List<string>();
      stringList5.Add("foot");
      CultureInfo culture5 = this._culture;
      ref string local5 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word5, (IEnumerable<string>) stringList5, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "eet"), culture5, out local5))
        return prefixWord + str;
      string word6 = suffixWord;
      List<string> stringList6 = new List<string>();
      stringList6.Add("zoon");
      CultureInfo culture6 = this._culture;
      ref string local6 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word6, (IEnumerable<string>) stringList6, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "oa"), culture6, out local6))
        return prefixWord + str;
      string word7 = suffixWord;
      List<string> stringList7 = new List<string>();
      stringList7.Add("cis");
      stringList7.Add("sis");
      stringList7.Add("xis");
      CultureInfo culture7 = this._culture;
      ref string local7 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word7, (IEnumerable<string>) stringList7, (Func<string, string>) (s => s.Remove(s.Length - 2, 2) + "es"), culture7, out local7))
        return prefixWord + str;
      if (this._assimilatedClassicalInflectionPluralizationService.ExistsInFirst(suffixWord))
        return prefixWord + this._assimilatedClassicalInflectionPluralizationService.GetSecondValue(suffixWord);
      if (this._classicalInflectionPluralizationService.ExistsInFirst(suffixWord))
        return prefixWord + this._classicalInflectionPluralizationService.GetSecondValue(suffixWord);
      string word8 = suffixWord;
      List<string> stringList8 = new List<string>();
      stringList8.Add("trix");
      CultureInfo culture8 = this._culture;
      ref string local8 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word8, (IEnumerable<string>) stringList8, (Func<string, string>) (s => s.Remove(s.Length - 1, 1) + "ces"), culture8, out local8))
        return prefixWord + str;
      string word9 = suffixWord;
      List<string> stringList9 = new List<string>();
      stringList9.Add("eau");
      stringList9.Add("ieu");
      CultureInfo culture9 = this._culture;
      ref string local9 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word9, (IEnumerable<string>) stringList9, (Func<string, string>) (s => s + "x"), culture9, out local9))
        return prefixWord + str;
      string word10 = suffixWord;
      List<string> stringList10 = new List<string>();
      stringList10.Add("inx");
      stringList10.Add("anx");
      stringList10.Add("ynx");
      CultureInfo culture10 = this._culture;
      ref string local10 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word10, (IEnumerable<string>) stringList10, (Func<string, string>) (s => s.Remove(s.Length - 1, 1) + "ges"), culture10, out local10))
        return prefixWord + str;
      string word11 = suffixWord;
      List<string> stringList11 = new List<string>();
      stringList11.Add("ch");
      stringList11.Add("sh");
      stringList11.Add("ss");
      CultureInfo culture11 = this._culture;
      ref string local11 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word11, (IEnumerable<string>) stringList11, (Func<string, string>) (s => s + "es"), culture11, out local11))
        return prefixWord + str;
      string word12 = suffixWord;
      List<string> stringList12 = new List<string>();
      stringList12.Add("alf");
      stringList12.Add("elf");
      stringList12.Add("olf");
      stringList12.Add("eaf");
      stringList12.Add("arf");
      Func<string, string> operationOnWord = (Func<string, string>) (s => !s.EndsWith("deaf", true, this._culture) ? s.Remove(s.Length - 1, 1) + "ves" : s);
      CultureInfo culture12 = this._culture;
      ref string local12 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word12, (IEnumerable<string>) stringList12, operationOnWord, culture12, out local12))
        return prefixWord + str;
      string word13 = suffixWord;
      List<string> stringList13 = new List<string>();
      stringList13.Add("nife");
      stringList13.Add("life");
      stringList13.Add("wife");
      CultureInfo culture13 = this._culture;
      ref string local13 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word13, (IEnumerable<string>) stringList13, (Func<string, string>) (s => s.Remove(s.Length - 2, 2) + "ves"), culture13, out local13))
        return prefixWord + str;
      string word14 = suffixWord;
      List<string> stringList14 = new List<string>();
      stringList14.Add("ay");
      stringList14.Add("ey");
      stringList14.Add("iy");
      stringList14.Add("oy");
      stringList14.Add("uy");
      CultureInfo culture14 = this._culture;
      ref string local14 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word14, (IEnumerable<string>) stringList14, (Func<string, string>) (s => s + nameof (s)), culture14, out local14))
        return prefixWord + str;
      if (suffixWord.EndsWith("y", true, this._culture))
        return prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1) + "ies";
      if (this._oSuffixPluralizationService.ExistsInFirst(suffixWord))
        return prefixWord + this._oSuffixPluralizationService.GetSecondValue(suffixWord);
      string word15 = suffixWord;
      List<string> stringList15 = new List<string>();
      stringList15.Add("ao");
      stringList15.Add("eo");
      stringList15.Add("io");
      stringList15.Add("oo");
      stringList15.Add("uo");
      CultureInfo culture15 = this._culture;
      ref string local15 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word15, (IEnumerable<string>) stringList15, (Func<string, string>) (s => s + nameof (s)), culture15, out local15))
        return prefixWord + str;
      return suffixWord.EndsWith("o", true, this._culture) || suffixWord.EndsWith("x", true, this._culture) ? prefixWord + suffixWord + "es" : prefixWord + suffixWord + "s";
    }

    /// <summary>Returns the singular form of the specified word.</summary>
    /// <returns>The singular form of the input parameter.</returns>
    /// <param name="word">The word to be made singular.</param>
    public string Singularize(string word) => EnglishPluralizationService.Capitalize(word, new Func<string, string>(this.InternalSingularize));

    private string InternalSingularize(string word)
    {
      if (this._userDictionary.ExistsInSecond(word))
        return this._userDictionary.GetFirstValue(word);
      if (this.IsNoOpWord(word))
        return word;
      string prefixWord;
      string suffixWord = EnglishPluralizationService.GetSuffixWord(word, out prefixWord);
      if (this.IsNoOpWord(suffixWord) || this.IsUninflective(suffixWord) || this._knownSingluarWords.Contains(suffixWord.ToLowerInvariant()))
        return prefixWord + suffixWord;
      if (this._irregularVerbPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._irregularVerbPluralizationService.GetFirstValue(suffixWord);
      if (this._irregularPluralsPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._irregularPluralsPluralizationService.GetFirstValue(suffixWord);
      if (this._wordsEndingWithSisPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._wordsEndingWithSisPluralizationService.GetFirstValue(suffixWord);
      if (this._wordsEndingWithSePluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._wordsEndingWithSePluralizationService.GetFirstValue(suffixWord);
      string word1 = suffixWord;
      List<string> stringList1 = new List<string>();
      stringList1.Add("men");
      CultureInfo culture1 = this._culture;
      string str;
      ref string local1 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word1, (IEnumerable<string>) stringList1, (Func<string, string>) (s => s.Remove(s.Length - 2, 2) + "an"), culture1, out local1))
        return prefixWord + str;
      string word2 = suffixWord;
      List<string> stringList2 = new List<string>();
      stringList2.Add("lice");
      stringList2.Add("mice");
      CultureInfo culture2 = this._culture;
      ref string local2 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word2, (IEnumerable<string>) stringList2, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "ouse"), culture2, out local2))
        return prefixWord + str;
      string word3 = suffixWord;
      List<string> stringList3 = new List<string>();
      stringList3.Add("teeth");
      CultureInfo culture3 = this._culture;
      ref string local3 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word3, (IEnumerable<string>) stringList3, (Func<string, string>) (s => s.Remove(s.Length - 4, 4) + "ooth"), culture3, out local3))
        return prefixWord + str;
      string word4 = suffixWord;
      List<string> stringList4 = new List<string>();
      stringList4.Add("geese");
      CultureInfo culture4 = this._culture;
      ref string local4 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word4, (IEnumerable<string>) stringList4, (Func<string, string>) (s => s.Remove(s.Length - 4, 4) + "oose"), culture4, out local4))
        return prefixWord + str;
      string word5 = suffixWord;
      List<string> stringList5 = new List<string>();
      stringList5.Add("feet");
      CultureInfo culture5 = this._culture;
      ref string local5 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word5, (IEnumerable<string>) stringList5, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "oot"), culture5, out local5))
        return prefixWord + str;
      string word6 = suffixWord;
      List<string> stringList6 = new List<string>();
      stringList6.Add("zoa");
      CultureInfo culture6 = this._culture;
      ref string local6 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word6, (IEnumerable<string>) stringList6, (Func<string, string>) (s => s.Remove(s.Length - 2, 2) + "oon"), culture6, out local6))
        return prefixWord + str;
      string word7 = suffixWord;
      List<string> stringList7 = new List<string>();
      stringList7.Add("ches");
      stringList7.Add("shes");
      stringList7.Add("sses");
      CultureInfo culture7 = this._culture;
      ref string local7 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word7, (IEnumerable<string>) stringList7, (Func<string, string>) (s => s.Remove(s.Length - 2, 2)), culture7, out local7))
        return prefixWord + str;
      if (this._assimilatedClassicalInflectionPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._assimilatedClassicalInflectionPluralizationService.GetFirstValue(suffixWord);
      if (this._classicalInflectionPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._classicalInflectionPluralizationService.GetFirstValue(suffixWord);
      string word8 = suffixWord;
      List<string> stringList8 = new List<string>();
      stringList8.Add("trices");
      CultureInfo culture8 = this._culture;
      ref string local8 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word8, (IEnumerable<string>) stringList8, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "x"), culture8, out local8))
        return prefixWord + str;
      string word9 = suffixWord;
      List<string> stringList9 = new List<string>();
      stringList9.Add("eaux");
      stringList9.Add("ieux");
      CultureInfo culture9 = this._culture;
      ref string local9 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word9, (IEnumerable<string>) stringList9, (Func<string, string>) (s => s.Remove(s.Length - 1, 1)), culture9, out local9))
        return prefixWord + str;
      string word10 = suffixWord;
      List<string> stringList10 = new List<string>();
      stringList10.Add("inges");
      stringList10.Add("anges");
      stringList10.Add("ynges");
      CultureInfo culture10 = this._culture;
      ref string local10 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word10, (IEnumerable<string>) stringList10, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "x"), culture10, out local10))
        return prefixWord + str;
      string word11 = suffixWord;
      List<string> stringList11 = new List<string>();
      stringList11.Add("alves");
      stringList11.Add("elves");
      stringList11.Add("olves");
      stringList11.Add("eaves");
      stringList11.Add("arves");
      CultureInfo culture11 = this._culture;
      ref string local11 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word11, (IEnumerable<string>) stringList11, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "f"), culture11, out local11))
        return prefixWord + str;
      string word12 = suffixWord;
      List<string> stringList12 = new List<string>();
      stringList12.Add("nives");
      stringList12.Add("lives");
      stringList12.Add("wives");
      CultureInfo culture12 = this._culture;
      ref string local12 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word12, (IEnumerable<string>) stringList12, (Func<string, string>) (s => s.Remove(s.Length - 3, 3) + "fe"), culture12, out local12))
        return prefixWord + str;
      string word13 = suffixWord;
      List<string> stringList13 = new List<string>();
      stringList13.Add("ays");
      stringList13.Add("eys");
      stringList13.Add("iys");
      stringList13.Add("oys");
      stringList13.Add("uys");
      CultureInfo culture13 = this._culture;
      ref string local13 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word13, (IEnumerable<string>) stringList13, (Func<string, string>) (s => s.Remove(s.Length - 1, 1)), culture13, out local13))
        return prefixWord + str;
      if (suffixWord.EndsWith("ies", true, this._culture))
        return prefixWord + suffixWord.Remove(suffixWord.Length - 3, 3) + "y";
      if (this._oSuffixPluralizationService.ExistsInSecond(suffixWord))
        return prefixWord + this._oSuffixPluralizationService.GetFirstValue(suffixWord);
      string word14 = suffixWord;
      List<string> stringList14 = new List<string>();
      stringList14.Add("aos");
      stringList14.Add("eos");
      stringList14.Add("ios");
      stringList14.Add("oos");
      stringList14.Add("uos");
      Func<string, string> operationOnWord = (Func<string, string>) (s => suffixWord.Remove(suffixWord.Length - 1, 1));
      CultureInfo culture14 = this._culture;
      ref string local14 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word14, (IEnumerable<string>) stringList14, operationOnWord, culture14, out local14))
        return prefixWord + str;
      string word15 = suffixWord;
      List<string> stringList15 = new List<string>();
      stringList15.Add("ces");
      CultureInfo culture15 = this._culture;
      ref string local15 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word15, (IEnumerable<string>) stringList15, (Func<string, string>) (s => s.Remove(s.Length - 1, 1)), culture15, out local15))
        return prefixWord + str;
      string word16 = suffixWord;
      List<string> stringList16 = new List<string>();
      stringList16.Add("ces");
      stringList16.Add("ses");
      stringList16.Add("xes");
      CultureInfo culture16 = this._culture;
      ref string local16 = ref str;
      if (PluralizationServiceUtil.TryInflectOnSuffixInWord(word16, (IEnumerable<string>) stringList16, (Func<string, string>) (s => s.Remove(s.Length - 2, 2)), culture16, out local16))
        return prefixWord + str;
      if (suffixWord.EndsWith("oes", true, this._culture))
        return prefixWord + suffixWord.Remove(suffixWord.Length - 2, 2);
      return suffixWord.EndsWith("ss", true, this._culture) || !suffixWord.EndsWith("s", true, this._culture) ? prefixWord + suffixWord : prefixWord + suffixWord.Remove(suffixWord.Length - 1, 1);
    }

    private bool IsPlural(string word)
    {
      if (this._userDictionary.ExistsInSecond(word))
        return true;
      if (this._userDictionary.ExistsInFirst(word))
        return false;
      return this.IsUninflective(word) || this._knownPluralWords.Contains(word.ToLower(this._culture)) || !this.Singularize(word).Equals(word);
    }

    private static string Capitalize(string word, Func<string, string> action)
    {
      string str = action(word);
      if (!EnglishPluralizationService.IsCapitalized(word) || str.Length == 0)
        return str;
      StringBuilder stringBuilder = new StringBuilder(str.Length);
      stringBuilder.Append(char.ToUpperInvariant(str[0]));
      stringBuilder.Append(str.Substring(1));
      return stringBuilder.ToString();
    }

    private static string GetSuffixWord(string word, out string prefixWord)
    {
      int num = word.LastIndexOf(' ');
      prefixWord = word.Substring(0, num + 1);
      return word.Substring(num + 1);
    }

    private static bool IsCapitalized(string word) => !string.IsNullOrEmpty(word) && char.IsUpper(word, 0);

    private static bool IsAlphabets(string word) => !string.IsNullOrEmpty(word.Trim()) && word.Equals(word.Trim()) && !Regex.IsMatch(word, "[^a-zA-Z\\s]");

    private bool IsUninflective(string word) => PluralizationServiceUtil.DoesWordContainSuffix(word, (IEnumerable<string>) this._uninflectiveSuffixes, this._culture) || !word.ToLower(this._culture).Equals(word) && word.EndsWith("ese", false, this._culture) || ((IEnumerable<string>) this._uninflectiveWords).Contains<string>(word.ToLowerInvariant());

    private bool IsNoOpWord(string word) => !EnglishPluralizationService.IsAlphabets(word) || word.Length <= 1 || this._pronounList.Contains(word.ToLowerInvariant());
  }
}
