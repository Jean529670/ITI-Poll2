import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { GraphQLResult, GraphQLService } from './graph-ql.service';
import { signUpSuccess } from './identity.service.actions';

export interface SignInResult {
  user: {
    signIn: SignInPayload
  }
}

export interface SignInPayload {
  errors: Error[],
  authentication: {
    user: {
      userId: number,
      email: string
    }
    accessToken: string
  }
}

const authFieldsFragment = `
  fragment authFields on SignInPayload {
    errors {
      type
      message
    }
    authentication {
      user {
        userId
        email
      }
      accessToken
    }
  }`;

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(
    private readonly graphQLService: GraphQLService,
    private readonly store: Store) { }

  authenticate(email: string, password: string) {
    const query = `
      mutation Login($login: SignInInput!) {
        user {
          signIn(login: $login) {
            ...authFields
          }
        }
      }
      
      ${authFieldsFragment}`;

    const result$ = this.graphQLService.send<SignInResult>(query, {
      login: { email, password }
    });

    return this.dispatchIdentityToStore(result$);
  }

  signUp(email: string, nickname: string, password: string) {
    const query = `
      mutation SignUp($login: SignUpInput!) {
        user {
          signIn: signUp(login: $login) {
            ...authFields
          }
        }
      }
      
      ${authFieldsFragment}`;

    const result$ = this.graphQLService.send<SignInResult>(query, {
      login: {
        email,
        nickname,
        password
      }
    });

    return this.dispatchIdentityToStore(result$);
  }

  dispatchIdentityToStore(result: Observable<GraphQLResult<SignInResult>>) {
    return result.pipe(
      tap(result => { 
        if ((typeof(result.errors) == 'undefined' || result.errors.length == 0)
          && result.data.user.signIn.errors.length == 0) {
          const authentication = result.data.user.signIn.authentication;
          const identity = {
            userId: authentication.user.userId,
            email: authentication.user.email,
            accessToken: authentication.accessToken
          };
          this.store.dispatch(signUpSuccess({ identity }));
        }
      })
    );
  }

}
