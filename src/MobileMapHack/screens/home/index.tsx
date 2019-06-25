import React from "react";
import {Container, Content, Text} from "native-base";
import Constants from "expo-constants";
import { HeaderBar } from "../../components/headerBar";

export const Home = (props) => {
    return (
        <Container style={{flex:1}}>
            <HeaderBar navigation={props.navigation} title="Home" showSearch={false} />
                
            <Text>Home screen</Text>
        </Container>
    )
};
