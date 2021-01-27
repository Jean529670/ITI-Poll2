import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { selectAccessToken } from './identity.selectors';

export interface Error {
  type: string,
  message: string
}

export interface GraphQLResult<T> {
  data: T,
  errors: {
   extensions: {
     code: string
   } 
  }[]
}

@Injectable({
  providedIn: 'root'
})
export class GraphQLService {

  constructor(
    private readonly httpClient: HttpClient,
    private readonly store: Store) { }

  send<T>(query: string, variables?: {[key: string]: any}, accessToken?: string): Observable<GraphQLResult<T>> {
    const body: any = { query };
    if (variables) body.variables = variables;
    const stringifiedBody = JSON.stringify(body);

    const headers: { [key: string]: string } = {
      'Content-Type': 'application/json'
    };

    if (accessToken) headers['Authorization'] = `Bearer ${accessToken}`;

    return this.httpClient.post<GraphQLResult<T>>(environment.apiUrl, stringifiedBody, {
      headers,
      responseType: 'json'
    });
  }

  secureSend<T>(query: string, variables?: {[key: string]: any}): Observable<GraphQLResult<T>> {
    return this.store
      .select(selectAccessToken)
      .pipe(
        mergeMap(accessToken => this.send<T>(query, variables, accessToken))
      );
  }
}
