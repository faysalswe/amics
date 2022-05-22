export class ApplicationSettings {
  public static ApiUrl: string='';   
  public publicApiUrl:string ='';
  constructor(url:string)
  {
    ApplicationSettings.ApiUrl = url;
  }
}
