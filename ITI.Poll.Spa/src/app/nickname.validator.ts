import { Injectable } from "@angular/core";
import { AbstractControl, AsyncValidator } from "@angular/forms";
import { first, map, tap } from "rxjs/operators";
import { UserService } from "./user.service";

@Injectable({ providedIn: 'root' })
export class NicknameValidator implements AsyncValidator {

  constructor(private readonly  userService: UserService) { }

  validate(ctrl: AbstractControl) {
    return this.userService.validateNickname(ctrl.value)
      .pipe(
        map(isValid => isValid ? null : { invalidNickname: { value: true } }),
        first());
  }

}