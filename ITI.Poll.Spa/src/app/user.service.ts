import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Error, GraphQLService } from './graph-ql.service';

interface CheckNicknameResult {
  checkNickname: {
    errors: Error[]
  }
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private readonly graphQLService: GraphQLService) { }

  validateNickname(nickname: string) {
    const query = `
      query CheckNickname($nickname: String!) {
        checkNickname(nickname: $nickname) {
          errors {
            type
          }
        }
      }`;

    return this.graphQLService
      .secureSend<CheckNicknameResult>(query, { nickname })
      .pipe(
        map(r => (typeof(r.errors) == 'undefined' || r.errors.length == 0) && r.data.checkNickname.errors.length == 0)
      );
  }
}
