import { TestBed } from "@angular/core/testing";
import { MockStore, provideMockStore } from "@ngrx/store/testing";
import { of } from "rxjs";
import { GraphQLResult, GraphQLService } from "./graph-ql.service";
import { IdentityService, SignInResult } from "./identity.service";
import { signUpSuccess } from "./identity.service.actions";

describe('IdentityService', () => {
    let graphQLServiceSpy: jasmine.SpyObj<GraphQLService>;
    let identityService: IdentityService;
    let store: MockStore;

    const emailUser = 'user@user.com';
    const passwordUser = 'testPAs1word';
    const badPassword = 'badPassword';

    const signUpResult: GraphQLResult<SignInResult> = {
        errors: [],
        data: {
            user: {
                signIn: {
                    errors: [],
                    authentication: {
                      user: {
                        userId: 1,
                        email: 'user@user.com'
                      },
                      accessToken: 'accesstoken1'
                    }
                }
            }
        }
    };

    const signUpResultError: GraphQLResult<SignInResult> = {
        errors: [],
        data: {
            user: {
                signIn: {
                    errors: [{
                        name: 'IncorectPassword',
                        message: 'Password is incorrect'
                    }],
                    authentication: {
                      user: {
                        userId: 0,
                        email: 'error'
                      },
                      accessToken: ''
                    }
                }
            }
        }
    };

    const initialState = {
        identity: {
            userId: 0,
            email: '',
            accessToken: ''
        },
    };

    beforeEach(() => {
        graphQLServiceSpy = jasmine.createSpyObj('GraphQLService', ['send']);
        TestBed.configureTestingModule({
          providers: [
            { provide: GraphQLService, useValue: graphQLServiceSpy },
            provideMockStore({ initialState })
          ]
        });
        identityService = TestBed.inject(IdentityService);
        store = TestBed.inject(MockStore);
        spyOn(store, 'dispatch');
    });

    describe('authenticate', () => {
        it('should set identity to the store if login are correct', (done) => {
            const graphQLService: unknown = {
                send() {
                    return of(signUpResult)
                }
            };
            identityService = new IdentityService(graphQLService as GraphQLService, store);
            identityService.authenticate(emailUser, passwordUser).subscribe(_ => {
                const identity = {
                    userId: 1,
                    email: emailUser,
                    accessToken: 'accesstoken1'
                };
                expect(store.dispatch).toHaveBeenCalledOnceWith(signUpSuccess({ identity }));
                done();
            });
        });

        it('should have to do nothing if login are incorrect', (done) => {
            const graphQLService: unknown = {
                send() {
                    return of(signUpResultError)
                }
            };
            identityService = new IdentityService(graphQLService as GraphQLService, store);
            identityService.authenticate(emailUser, passwordUser).subscribe(_ => {
                expect(store.dispatch).not.toHaveBeenCalled();
                done();
            });
        });
    });

    describe('signUp', () => {
        const nickName = 'nickTestName';
        it('should set identity to the store if login are correct', (done) => {
            const graphQLService: unknown = {
                send() {
                    return of(signUpResult)
                }
            };
            identityService = new IdentityService(graphQLService as GraphQLService, store);
            identityService.signUp(emailUser, nickName, badPassword).subscribe(_ => {
                const identity = {
                    userId: 1,
                    email: emailUser,
                    accessToken: 'accesstoken1'
                };
                expect(store.dispatch).toHaveBeenCalledOnceWith(signUpSuccess({ identity }));
                done();
            });
        });

        it('should have to do nothing if login are incorrect', (done) => {
            const graphQLService: unknown = {
                send() {
                    return of(signUpResultError)
                }
            };
            identityService = new IdentityService(graphQLService as GraphQLService, store);
            identityService.signUp(emailUser, nickName, badPassword).subscribe(_ => {
                expect(store.dispatch).not.toHaveBeenCalled();
                done();
            });
        });
    });
});