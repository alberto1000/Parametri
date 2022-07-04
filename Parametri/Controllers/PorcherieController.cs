using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
/*
 * Passaggio dei parametri e routing
 * Ricordiamo che per overload un'azione può avere lo stesso nome di un'altra ma usare parametri diversi
 * Noi possiamo passare i parametri ad un'azione entry point del nostro controller nei seguenti modi:
 *  -01- attraverso il routing usando FromRouteAttribute
 *  -02- attraverso l'url usando FromQuery (bisogna assegnare nomi ai parametri e usarli)
 *  -03- tramite il Body fornedo nel Body della richiesta un oggetto in formato JSON o XML
 *  Nel nostro caso abbiamo l'informazione di base  [Route("api/[controller]")] -> api/Porcherie,
 *  quando lanciamo una richesta costruiremo la base dell'Url specificando il protocollo http o https,
 *  l'host (la locazione del servizio web) e le caratteristiche di routing, nel nostro caso avremo:
 *       http://localhost:11000/api/Porcherie
 *   a questa URL di riferimento possiamo aggiungere parametri e specificare il metodo http/s da utilizzare.
 *  Ad esempio per la GET possiamo costruire i seguenti metodi:
 *    -A- Usando FromRouteAttribute:
 *       -01- public ActionResult<List<Porcheria>> Get() che verrà chiamato con http://localhost:11000/api/Porcherie
 *            e sarà annotato con [HttpGet]
 *       -02- public ActionResult<Porcheria> Get(int id) che verrà chiamato con http://localhost:11000/api/Porcherie/<valore id>
 *            nel nostro caso valore è un intero ad es. 3; il metodo verrà annotato con [HttpGet("{id}")]
 *       -03- public ActionResult<Porcheria> Get(int id, string bonta) che verrà chiamato con
 *            http://localhost:11000/api/Porcherie/<valore id>/<valore bonta> e verrà annotato con 
 *            [HttpGet("{id}/{bonta}")]
 *    -B- Usando FromQuery.
 *        Attenzione abbiamo definito già tre metodi get con parametri differenti per applicare FromRouteAttribute
 *        l'overload non ci permetterà di usare un altro metodo get con 0,1 o 2 parametri dobbiamo cambiare il nome
 *        dell'azione, la chiameremo dettaglio però biogna anche stare attenti al Routing attualmente il nostro si basa su
 *        [Route("api/[controller]")] quindi http://localhost:11000/api/Porcherie/ ed idividua l'azione Get che abbiamo scritto
 *        in funione dei parametri di route che abbiamo definito per l'azione. Volendo aggiungere altre azioni che implementano la get
 *        con nome metodo diverso e con gli stessi parametri delle get precedenti forniti da una query string dobbiamo modificare 
 *        anche il nostro route aggiungendo un action [Route("api/[controller]/[action]")] questo vuol dire che potrò usare il nome del
 *        metodo per invocare (un nome metodo per ogni operazione ). Fatto ciò le chiamate precedenti diventeranno:
 *             -- http://localhost:11000/api/Porcherie/Get
 *             -- http://localhost:11000/api/Porcherie/Get/<valore id>
 *             -- http://localhost:11000/api/Porcherie/Get/<valore id>/<valore bonta> 
 *         Naturalmente senza cambiare nulla si sarebbe potuto costruire un altro controller (ad es. PorcherieQ) che usava le query string.
 *         Fatto ciò impostiamo i metodi che gestiranno la get usando un altro nome di metodo:
 *        -01- public ActionResult<Porcheria> Dettaglio(int id) la annoteremo con [HttpGet("dettaglio")] perciò
 *             a tutti gli effetti sarà una get; la chiameremo usando query string così
 *             http://localhost:11000/api/Porcherie/dettaglio?id=3 dopo il nome dell'azione inseriamo il ? e la coppia
 *             <chiave>=<valore>
 *        -02- public ActionResult<Porcheria> dettaglio(int id, string bonta) l'annoteremo con [HttpGet("dettaglio")] perciò
 *             a tutti gli effetti sarà una get; la chiameremo usando query string così
 *             http://localhost:11000/api/Porcherie/dettaglio?id=3&bonta=Buona dopo il nome dell'azione inseriamo il ? e la coppia
 *             <chiave>=<valore> quindi & e di la coppia <chiave>=<valore>
 *      In quest'esempio non sono stati implementati altri metodi. Le prove sono state fatte con Crome:
 *      -01- http://localhost:11000/api/Porcherie/Get risultato:
 *           [{"id":1,"bonta":"Buona"},{"id":2,"bonta":"Buonissima"},{"id":3,"bonta":"Cattiva"},{"id":4,"bonta":"Cattivissima"},{"id":5,"bonta":"Spregevole"},{"id":6,"bonta":"Spregevolissima"}]
 *      -02- http://localhost:11000/api/Porcherie/Get/1 risultato: {"id":1,"bonta":"Buona"}
 *      -03- http://localhost:11000/api/Porcherie/Get/4/Cattivissima risultato: {"id":4,"bonta":"Cattivissima"}
 *      -04- http://localhost:11000/api/Porcherie/dettaglio?id=1 riusltato: {"id":1,"bonta":"Buona"}
 *      -05- http://localhost:11000/api/Porcherie/altrodettaglio?id=5&bonta=Spregevole risultato: {"id":5,"bonta":"Spregevole"}
 * */
namespace Parametri.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PorcherieController : ControllerBase
    {
        // GET: api/<PorcherieController>/Get
        [HttpGet]
        public ActionResult<List<Porcheria>> Get()
        {
            return creaPorcherie();
        }

        // GET api/<PorcherieController>/Get/5
        // From Route
        [HttpGet("{id}")]
        public ActionResult<Porcheria> Get(int id)
        {
            List<Porcheria> lp = creaPorcherie();

            return lp.Where(xyz => xyz.id == id).FirstOrDefault();
        }
        // GET api/<PorcherieController>/dettaglio?id=5
        // From Query
        [HttpGet]
        
        public ActionResult<Porcheria> Dettaglio([FromQuery] int id)
        {
            List<Porcheria> lp = creaPorcherie();

            return lp.Where(xyz => xyz.id == id).FirstOrDefault();
        }

        // GET api/<PorcherieController>/Get/5/Buona
        // From Route
        [HttpGet("{id}/{bonta}")]
        public ActionResult<Porcheria> Get( int id, string bonta)
        {
            List<Porcheria> lp = creaPorcherie();
            int xyz = 3;
            return lp.Where(xyz => xyz.id == id && xyz.bonta == bonta).FirstOrDefault();
        }

        // GET api/<PorcherieController>/Dettaglio?id=5&&bonta=Buona
        // From Query
        [HttpGet]
        public ActionResult<Porcheria> AltroDettaglio([FromQuery] int id, [FromQuery] string bonta)
        {
            List<Porcheria> lp = creaPorcherie();
            Porcheria po = lp.Where(xyz => xyz.id == id && xyz.bonta == bonta).FirstOrDefault();
            if (po == null) return Problem("Non so cosa mi hai dato, ma io non l'ho trovato");
            return po;
        }


        // POST api/<PorcherieController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PorcherieController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //non implementata
        }

        // DELETE api/<PorcherieController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private List<Porcheria> creaPorcherie()
        {
            List<Porcheria> lp = new()
            {
                new Porcheria { bonta = "Buona", id = 1 },
                new Porcheria { bonta = "Buonissima", id = 2 },
                new Porcheria { bonta = "Cattiva", id = 3 },
                new Porcheria { bonta = "Cattivissima", id = 4 },
                new Porcheria { bonta = "Spregevole", id = 5 },
                new Porcheria { bonta = "Spregevolissima", id = 6 }
            };
            return lp;

        }
    }
}
