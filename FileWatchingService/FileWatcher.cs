using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSXML2;
namespace FileWatchingService
{
    public class FileWatcher
    {
        private FileSystemWatcher _fileWatcher;
        static string nombreFile = string.Empty;
        static readonly string directorio = ConfigurationManager.AppSettings["RouteTxt"];
        static readonly string directorioEnviados = directorio + @"enviados\";

        public FileWatcher()
        {
            Init();
        }

        private void Init()
        {
            _fileWatcher = new FileSystemWatcher(Route());
            _fileWatcher.Filter = "*.txt";
            _fileWatcher.Created += new FileSystemEventHandler(_fileWatcher_Created);
            _fileWatcher.Changed += new FileSystemEventHandler(_fileWatcher_Changed);
            _fileWatcher.Renamed += new RenamedEventHandler(_fileWatcher_Renamed);
            _fileWatcher.EnableRaisingEvents = true;

        }

        private void _fileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            //Logger.Log(string.Format("Archivo DTE Renombrado: Ruta: {0}, Nombre: {1}", e.FullPath, e.Name));
            try
            {
                ProcFiles(e);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Excepcion de watcher renamed: " + ex));
            }
        }

        private void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //Logger.Log(string.Format("Archivo DTE cambiado: Ruta: {0}, Nombre: {1}", e.FullPath, e.Name));
            try
            {
                ProcFiles(e);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Excepcion de watcher Changed: " + ex));
            }
        }

        private void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //Logger.Log(string.Format("Archivo DTE creado: Ruta: {0}, Nombre: {1}", e.FullPath, e.Name));
            try
            {
                ProcFiles(e);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Excepcion de watcher cREATED: " + ex));
            }
        }

        private static void ProcFiles(FileSystemEventArgs e)
        {
            string[] archivosTxt = Directory.GetFiles(directorio, "*.txt").Select(Path.GetFileName).ToArray();
            foreach (string archivosPlanos in archivosTxt)
            {
                Logger.Log(string.Format("NetDte: Archivo Construido: {0}", e.Name));
                nombreFile = archivosPlanos;
                Metamorphosis(e);
            }
        }

        private static void Metamorphosis(FileSystemEventArgs e)
        {
            #region Variables
            string indExento = string.Empty, nombreItem = string.Empty, descriItem = string.Empty;
            string porcentajeDescuentoItem = string.Empty, cantidadItem = string.Empty;
            string unidadItem = string.Empty, valorItem = string.Empty;
            string montoDescuentoItem = string.Empty, porcentajeRecargoItem = string.Empty;
            string montoRecargoItem = string.Empty, codImp1 = string.Empty, codImp2 = string.Empty;
            string totalItem = string.Empty, codigoItem = string.Empty, otraMoneda = string.Empty;
            string precioMoneda = string.Empty, factorMoneda = string.Empty, codigoMoneda = string.Empty;
            string codigoDocLiq = string.Empty;
            string tipoMov = string.Empty, GlosaDR = string.Empty, TpoValor = string.Empty, ValorDR = string.Empty, IndExeDR = string.Empty;
            string TpoDocRef = string.Empty, IndGlobal = string.Empty, FolioRef = string.Empty, FchRef = string.Empty, CodRef = string.Empty, RazonRef = string.Empty, RutOtroC = string.Empty;
            string TipoMovim = string.Empty, GlosaCom = string.Empty, TasaComision = string.Empty, ValComNeto = string.Empty, ValComExe = string.Empty, ValComIVA = string.Empty;
            string datos = string.Empty;
            string[] separador = { ";" };
            string[] separadorLineas = { "~" };
            string[] lines = File.ReadAllLines(e.FullPath);

            string[] aCabecera = lines[0].Replace("[", "").Replace("]", "").Split(separador, StringSplitOptions.None);
            string[] aEmisor = lines[1].Replace("[", "").Replace("]", "").Split(separador, StringSplitOptions.None);
            string[] aReceptor = lines[2].Replace("[", "").Replace("]", "").Split(separador, StringSplitOptions.None);
            string[] aTransporte = lines[3].Replace("[", "").Replace("]", "").Split(separador, StringSplitOptions.None);
            string[] aTotales = lines[4].Replace("[", "").Replace("]", "").Split(separador, StringSplitOptions.None);
            string[] aDetalle = lines[5].Replace("[", "").Replace("]", "").Split(separadorLineas, StringSplitOptions.None);
            string[] aDescRec = lines[6].Replace("[", "").Replace("]", "").Split(separadorLineas, StringSplitOptions.None);
            string[] aReferencia = lines[7].Replace("[", "").Replace("]", "").Split(separadorLineas, StringSplitOptions.None);
            string[] aComisiones = lines[8].Replace("[", "").Replace("]", "").Split(separadorLineas, StringSplitOptions.None);
            
            #endregion
            #region datos
            datos = "proservice=3&token=&id=" + ConfigurationManager.AppSettings["IdContribuyente"] + "&etapa=0&formato=&" +
                //Encabezado
                "folioDocumento=" + aCabecera[1] +
                "&tipoDte=" + aCabecera[0] +
                "&fec_emision=" + aCabecera[2] +
                "&indicador_no_rebaja_o_servicio=" + aCabecera[3] +
                "&tipo_despacho=" + aCabecera[4] +
                "&tipo_traslado=" + aCabecera[5] +
                "&forma_pago=" + aCabecera[6] +
                "&fec_vencimiento" + aCabecera[7] +
                "&correo_cliente=" + aCabecera[8] +
                "&fechaTimbre=" + aCabecera[9] +
                //Emisor
                "&rut_emisor=" + aEmisor[0] +
                "&razon_social=" + aEmisor[1] +
                "&giro=" + aEmisor[2] +
                "&codigo_actividad=" + aEmisor[3] +
                "&sucursal=" + aEmisor[4] +
                "&codigo_sucursal=" + aEmisor[5] +
                "&direccion_sucursal=" + aEmisor[6] +
                "&comuna_origen=" + aEmisor[7] +
                "&ciudad_origen=" + aEmisor[8] +
                "&codigo_vendedor=" + aEmisor[9] +
                //Receptor
                "&rut_receptor=" + aReceptor[0] +
                "&codigo_interno=" + aReceptor[1] +
                "&razon_social_recep=" + aReceptor[2] +
                "&giro_receptor=" + aReceptor[3] +
                "&contacto=" + aReceptor[4] +
                "&direccion_receptor=" + aReceptor[5] +
                "&comuna_recep=" + aReceptor[6] +
                "&ciudad_recep=" + aReceptor[7] +
                "&email_recep=" + aReceptor[8] +
                //transporte
                "&patente=" + aTransporte[0] +
                "&rut_transportista=" + aTransporte[1] +
                "&direccion_destino=" + aTransporte[2] +
                "&comuna_destino=" + aTransporte[3] +
                "&ciudad_destino=" + aTransporte[4] +
                //totales
                "&monto_neto=" + aTotales[0] +
                "&monto_exento=" + aTotales[1] +
                "&tasa_iva=" + aTotales[2] +
                "&iva=" + aTotales[3] +
                "&total=" + aTotales[4] +
                "&timestamp=" + aTotales[5] +
                "&codigo_imp_adicional1=" + aTotales[6] + "&tasa_imp_adicional1=" + aTotales[7] + "&monto_imp_adicional1=" + aTotales[8] +
                "&codigo_imp_adicional2=" + aTotales[9] + "&tasa_imp_adicional2=" + aTotales[10] + "&monto_imp_adicional2=" + aTotales[11] +
                "&codigo_imp_adicional3=" + aTotales[12] + "&tasa_imp_adicional3=" + aTotales[13] + "&monto_imp_adicional3=" + aTotales[14] +
                "&codigo_imp_adicional4=" + aTotales[15] + "&tasa_imp_adicional4=" + aTotales[16] + "&monto_imp_adicional4=" + aTotales[17] +
                "&codigo_imp_adicional5=" + aTotales[18] + "&tasa_imp_adicional5=" + aTotales[19] + "&monto_imp_adicional5=" + aTotales[20] +
                "&iva_no_retenido=" + aTotales[21] + "&monto_no_facturable=" + aTotales[22] + "&ceec=" + aTotales[23] +
                "&neto_comision=" + aTotales[24] + "&exento_comision=" + aTotales[25] + "&iva_comision=" + aTotales[26];
            //Detalle :o

            for (int i = 0; i < aDetalle.Length; i++)
            {
                string[] aDetDetalle = aDetalle[i].Split(separador, StringSplitOptions.None);
                indExento += aDetDetalle[0] + "|";
                nombreItem += aDetDetalle[1] + "|";
                descriItem += aDetDetalle[2] + "|";
                cantidadItem += aDetDetalle[3] + "|";
                unidadItem += aDetDetalle[4] + "|";
                valorItem += aDetDetalle[5] + "|";
                porcentajeDescuentoItem += aDetDetalle[6] + "|";
                montoDescuentoItem += aDetDetalle[7] + "|";
                porcentajeRecargoItem += aDetDetalle[8] + "|";
                montoRecargoItem += aDetDetalle[9] + "|";
                codImp1 += aDetDetalle[10] + "|";
                codImp2 += aDetDetalle[11] + "|";
                totalItem += aDetDetalle[12] + "|";
                codigoItem += aDetDetalle[13] + "|";
                otraMoneda += aDetDetalle[14] + "|";
                precioMoneda += aDetDetalle[15] + "|";
                factorMoneda += aDetDetalle[16] + "|";
                codigoMoneda += aDetDetalle[17] + "|";
                codigoDocLiq += aDetDetalle[18] + "|";
            }
            datos += "&indExento=" + indExento + "&nombreItem=" + nombreItem + "&descriItem=" + descriItem +
                "&cantidadItem=" + cantidadItem + "&unidadItem=" + unidadItem + "&valorItem=" + valorItem +
                "&porcentajeDescuentoItem=" + porcentajeDescuentoItem + "&montoDescuentoItem=" + montoDescuentoItem +
                "&porcentajeRecargoItem=" + porcentajeRecargoItem + "&montoRecargoItem=" + montoRecargoItem +
                "&codImp1=" + codImp1 + "&codImp2=" + codImp2 + "&totalItem=" + totalItem + "&codigoItem=" + codigoItem +
                "&otraMoneda=" + otraMoneda + "&precioMoneda=" + precioMoneda + "&factorMoneda=" + factorMoneda +
                "&codigoMoneda=" + codigoMoneda + "&codigoDocLiq=" + codigoDocLiq;

            //Descuento o Recargo
            for (int a = 0; a < aDescRec.Length; a++)
            {
                string[] aDetDesc = aDescRec[a].Split(separador, StringSplitOptions.None);
                tipoMov += aDetDesc[0] + "|";
                GlosaDR += aDetDesc[1] + "|";
                TpoValor += aDetDesc[2] + "|";
                ValorDR += aDetDesc[3] + "|";
                IndExeDR += aDetDesc[4] + "|";
            }
            datos += "&tipoMov=" + tipoMov + "&GlosaDR=" + GlosaDR + "&TpoValor=" + TpoValor + "&ValorDR=" + ValorDR +
                "&IndExeDR=" + IndExeDR;

            //Referencias
            for (int b = 0; b < aReferencia.Length; b++)
            {
                string[] aDetRef = aReferencia[b].Split(separador, StringSplitOptions.None);
                TpoDocRef += aDetRef[0] + "|";
                IndGlobal += aDetRef[1] + "|";
                FolioRef += aDetRef[2] + "|";
                FchRef += aDetRef[3] + "|";
                CodRef += aDetRef[4] + "|";
                RazonRef += aDetRef[5] + "|";
                RutOtroC += aDetRef[6] + "|";
            }
            datos += "&TpoDocRef=" + TpoDocRef + "&IndGlobal=" + IndGlobal + "&FolioRef=" + FolioRef + "&FchRef=" + FchRef +
                "&CodRef=" + CodRef + "&RazonRef=" + RazonRef + "&RutOtroC=" + RutOtroC;

            //Comisiones
            for (int c = 0; c < aComisiones.Length; c++)
            {
                string[] aDetCom = aComisiones[c].Split(separador, StringSplitOptions.None);
                TipoMovim += aDetCom[0] + "|";
                GlosaCom += aDetCom[1] + "|";
                TasaComision += aDetCom[2] + "|";
                ValComNeto += aDetCom[3] + "|";
                ValComExe += aDetCom[4] + "|";
                ValComIVA += aDetCom[5] + "|";
            }
            datos += "&TipoMovim=" + TipoMovim + "&GlosaCom=" + GlosaCom + "&TasaComision=" + TasaComision +
                "&ValComNeto=" + ValComNeto + "&ValComExe=" + ValComExe + "&ValComIVA=" + ValComIVA;
            #endregion
            SendPost(datos);
        }
        private static void SendPost(string datos)
        {          
            try
            {
                if (!Directory.Exists(directorioEnviados))
                    Directory.CreateDirectory(directorioEnviados);

                XMLHTTP netdte = new XMLHTTP();
                netdte.open("POST", "http://netdte.cl/ws/", false);
                netdte.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                netdte.setRequestHeader("Content-Length", datos.Length.ToString());
                netdte.send("func=generaDoc&" + datos);
                
                if (netdte.status != 200)
                {
                    Logger.Log(string.Format("Status: " + netdte.statusText));
                    Logger.Log(string.Format("Reintentando enviar txt"));
                    SendPost(datos);
                }
                else if (netdte.status == 200)
                {
                    System.Threading.Thread.Sleep(2000);
                    
                    if (File.Exists(directorioEnviados + nombreFile))
                        File.Delete(directorioEnviados + nombreFile);                    
                    try
                    {                        
                        File.Move(directorio + nombreFile, directorioEnviados + nombreFile);                        
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(string.Format("Catch File.move: " + ex));
                    }
                    
                }
                //Console.WriteLine(netdte.responseText);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Tuviste una Excepcion: " + ex));
            }
        }


        private string Route()
        {
            string value = string.Empty;

            try
            {
                value = ConfigurationManager.AppSettings["RouteTxt"];
            }
            catch (Exception ex)
            {
                Logger.Log("Mensaje de excepcion :" + ex.ToString());
            }
            return value;

        }

       

    }

   
}
