import React from 'react';
import { JourneySettings } from '../domain/types';

let initialState : JourneySettings = {
    showPollution: false,
    showSchools: false,
    startTime: '12:00',
    togglePollution: (showPollution: boolean) => {},
    toggleSchools: (showSchools: boolean) => {},
    toggleStartTime: (startTime: string) => {}
}

let JourneyContext = React.createContext(initialState);

export const JourneyProvider = JourneyContext.Provider;
export const JourneyConsumer = JourneyContext.Consumer;
export default JourneyContext;