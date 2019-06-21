import React from 'react';
import { JourneySettings } from '../domain/types';
// maybe this should just go in App.tsx
let JourneyContext = React.createContext<JourneySettings>(null);
export default JourneyContext;
