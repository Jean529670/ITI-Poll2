import { TestBed } from "@angular/core/testing";
import { GraphQLService } from "./graph-ql.service";
import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing";
import { MockStore } from "@ngrx/store/testing"
import { Store } from "@ngrx/store";
import { environment } from '../environments/environment';

describe('GraphQLService', () => {
  let sut: GraphQLService;
  let httpTestingController: HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        { provide: Store, useValue: MockStore }
      ]
    });

    sut = TestBed.inject(GraphQLService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(sut).toBeTruthy();
  });

  it('sends POST requests', () => {
    sut.send('query { test }')
      .subscribe(_ => {

      });

    const req = httpTestingController.expectOne(environment.apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush({
      errors: [],
      data: {
        test: 'test'
      }
    });
  });
})