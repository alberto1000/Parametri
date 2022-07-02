# Parametri
Esempio su uso di Api C# Asp Net Core 
* Passaggio dei parametri e routing
 * Ricordiamo che per overload un'azione può avere lo stesso nome di un'altra ma usare parametri diversi
 * Noi possiamo passare i parametri ad un'azione entry point del nostro controller nei seguenti modi:
 *  -01- attraverso il routing usando FromRouteAttribute
 *  -02- attraverso l'url usando FromQuery
 *  -03- tramite il Body fornedo nel Body della richiesta un oggetto in formato JSON o XML
