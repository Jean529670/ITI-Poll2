import { Injectable } from '@angular/core';
import { GraphQLService } from './graph-ql.service';
import { Error } from './graph-ql.service';

export interface Answer {
  text: string,
  votesCount: number
}

export interface Poll {
  pollId: number,
  question: string,
  answers: Answer[],
  guestCount: number
}

export interface Invitation {
  invitationId: number,
  question: string,
  proposals: {
    proposalId: number
    text: string
  }[]
}

export interface Me {
  polls: Poll[],
  invitations: Invitation[]
}

export interface MyPollResult {
  me: Me
}

export interface PollInput {
  question: string,
  proposals: string[],
  guests: string[]
}

export interface PollPayload {
  errors: Error[],
  poll: Poll
}

export interface CreatePollResult {
  poll: {
    createPoll: PollPayload
  }
}

export interface GetPollByIdResult {
  poll: {
    errors: Error[],
    poll: Poll
  }
}

export interface GetInvitationByIdResult {
  invitation: {
    errors: Error[],
    invitation: Invitation
  }
}

export interface VoteResult {
  errors: Error[]
}

@Injectable({
  providedIn: 'root'
})
export class PollService {

  constructor(private readonly graphQLService: GraphQLService) { }

  getMyPolls() {
    const query =
      `query MyPolls {
        me {
          polls {
            pollId
            question
          }
          invitations {
            invitationId
            question
          }
        }
      }`;

    return this.graphQLService.secureSend<MyPollResult>(query);
  }

  createPoll(poll: PollInput) {
    const query =
      `mutation CreatePoll($poll: PollInput!) {
        poll {
          createPoll(poll: $poll) {
            errors {
              type
              message
            }
            poll {
              pollId
            }
          }
        }
      }`;

    return this.graphQLService.secureSend<CreatePollResult>(query, { poll });
  }

  getPollById(pollId: number) {
    const query =
      `query GetPollById($pollId: ID!) {
        poll(pollId: $pollId) {
          errors {
            type
            message
          }
          poll {
            pollId
            question
            answers {
              answerId
              text
              votesCount
            }
            guestCount
          }
        }
      }`;

    return this.graphQLService.secureSend<GetPollByIdResult>(query, { pollId });
  }

  getInvitationById(invitationId: number) {
    const query =
      `query GetInvitationById($invitationId: ID!) {
        invitation(invitationId: $invitationId) {
          errors {
            type
            message
          }
          invitation {
            invitationId
            question
            proposals {
              proposalId
              text
            }
          }
        }
      }`;

    return this.graphQLService.secureSend<GetInvitationByIdResult>(query, { invitationId });
  }

  vote(invitationId: number, proposalId: number) {
    const query =
      `mutation Vote($vote: VoteInput!) {
        poll {
          vote(vote: $vote) {
            errors {
              type
              message
            }
          }
        }
      }`;

    return this.graphQLService.secureSend<VoteResult>(query, {
      vote: {
        pollId: invitationId,
        proposalId: proposalId
      }
    });
  }
}
