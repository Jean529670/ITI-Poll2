import { TestBed } from "@angular/core/testing";
import { of } from "rxjs";
import { GraphQLResult, GraphQLService } from "./graph-ql.service";
import { MyPollResult, PollService } from "./poll.service";

describe('PollService', () => {
  let graphQLServiceSpy: jasmine.SpyObj<GraphQLService>;
  let sut: PollService;

  beforeEach(() => {
    graphQLServiceSpy = jasmine.createSpyObj('GraphQLService', ['secureSend']);
    TestBed.configureTestingModule({
      providers: [
        { provide: GraphQLService, useValue: graphQLServiceSpy }
      ]
    });
    sut = TestBed.inject(PollService);
  });

  it('can get my polls', () => {
    const me: GraphQLResult<MyPollResult> = {
      errors: [],
      data: {
        me: {
          polls: [],
          invitations: []
        }
      }
    };
    const graphQLService: unknown = {
      secureSend() {
        return of(me)
      }
    };
    const sut: PollService = new PollService(graphQLService as GraphQLService);

    sut.getMyPolls()
      .subscribe(result => {
        expect(result).toBe(me);
      });
  });

  it('can get my polls (with spies)', () => {
    const me: GraphQLResult<MyPollResult> = {
      errors: [],
      data: {
        me: {
          polls: [],
          invitations: [
            { invitationId: 3712, question: 'Question?', proposals: [] }
          ]
        }
      }
    };
    const graphQLServiceSpy: jasmine.SpyObj<GraphQLService> =
      jasmine.createSpyObj<GraphQLService>('GraphQLService', ['secureSend']);
    graphQLServiceSpy.secureSend.and.returnValue(of(me));
    const sut = new PollService(graphQLServiceSpy);

    sut.getMyPolls()
      .subscribe(result => {
        expect(result).toBe(me);
      });
  });

  it('calls secureSend correctly', () => {
    const graphQLServiceSpy: jasmine.SpyObj<GraphQLService> =
      jasmine.createSpyObj<GraphQLService>('GraphQLService', ['secureSend']);
    graphQLServiceSpy.secureSend.and.returnValue(of({
      errors: [],
      data: {
        me: {
          polls: [],
          invitations: []
        }
      }
    }));
    const sut = new PollService(graphQLServiceSpy);
    sut.getMyPolls()
      .subscribe(_ => {
        expect(graphQLServiceSpy.secureSend).toHaveBeenCalledTimes(1);
      });
  });

  it('calls secureSend correctly (with TestBed)', () => {
    graphQLServiceSpy.secureSend.and.returnValue(of({
      errors: [],
      data: {
        me: {
          polls: [],
          invitations: []
        }
      }
    }));

    sut.getMyPolls()
      .subscribe(_ => {
        expect(graphQLServiceSpy.secureSend).toHaveBeenCalledTimes(1);
      });
  });

  it('calls secureSend correctly to create a new poll', () => {
    graphQLServiceSpy.secureSend.and.returnValue(of({
      errors: [],
      data: {}
    }));

    sut.createPoll({
      question: 'Question?',
      proposals: [ 'P1', 'P2' ],
      guests: [ 'Titi', 'Toto' ]
    }).subscribe(_ => {
      expect(graphQLServiceSpy.secureSend).toHaveBeenCalledOnceWith(jasmine.any(String), {
        poll: {
          question: 'Question?',
          proposals: [ 'P1', 'P2' ],
          guests: [ 'Titi', 'Toto' ]
        }
      });
    })
  });
})