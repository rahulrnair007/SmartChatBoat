import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import { environment } from '../../environments/environment';

@Injectable()
export class DialogflowService 
{

  public baseURL: string = "https://dialogflow.googleapis.com/v2/projects/";
  public accessToken: string = environment.token;

  constructor(private http: Http){}

  public getResponse(query: string)
  {
      let userData = 
      {
        queryData: query,
        sessionid: '12345',
      }

       return this.http.post(
           //'https://cowi-smartchatbot.azurewebsites.net/SmartChatBot/GetIntents',
        // 'https://localhost:44318/SmartChatBotController/GetIntents',
        'http://localhost:5000/SmartChatBot/GetIntents',
        userData
      ).map(res => 
            {
            return res.json()
          })
  }
}
